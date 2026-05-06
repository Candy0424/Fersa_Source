using System.Collections.Generic;
using CIW.Code;
using PSB_Lib.Dependencies;
using PSB_Lib.ObjectPool.RunTime;
using PSB.Code.BattleCode.Events;
using PSW.Code.EventBus;
using UnityEngine;
using Work.YIS.Code.Buffs;
using YIS.Code.Buffs.Enchants;
using YIS.Code.Defines;

namespace YIS.Code.Modules
{
    public struct BuffInfo
    {
        public int BuffKey;
        public float Value;
    }
    
    public class BuffModule : MonoBehaviour, IModule, IAfterInitModule
    {
        [SerializeField] private Entity uiAnchor;
        [SerializeField] private BuffListSO buffList;
        [SerializeField] private Transform parentRoot;

        [Inject] private PoolManagerMono _poolManager;

        private Entity _owner;

        private Dictionary<int, BuffVisualSO> _uiBuffTable;
        private Dictionary<int, BuffDataSO> _buffTable;
        private Dictionary<BuffInfo, Buff> _activeBuffTable;
        private List<BuffInfo> _disabledBuffBuffer;

        private EnchantInfo _skillEnchantOverride;
        private EnchantInfo _skillImmediatelyEnchantOverride;

        public Entity UiTarget => uiAnchor;

        public void Initialize(ModuleOwner owner)
        {
            _uiBuffTable = new Dictionary<int, BuffVisualSO>();
            _buffTable = new Dictionary<int, BuffDataSO>();
            _activeBuffTable = new Dictionary<BuffInfo, Buff>();
            _disabledBuffBuffer = new List<BuffInfo>();
            _skillEnchantOverride = new EnchantInfo(Elemental.None, 0);
            _skillImmediatelyEnchantOverride = new EnchantInfo(Elemental.None, 0);

            _owner = owner as Entity;
        }

        public void AfterInitialize()
        {
            BuffDataSO[] buffs = buffList.buffs;
            foreach (BuffDataSO buff in buffs)
            {
                if (buff.visualData == null) continue;
                _uiBuffTable.Add(buff.index, buff.visualData);
                _buffTable.Add(buff.index, buff);
            }
        }

        public void SetPool(PoolManagerMono poolManager)
        {
            if (_poolManager != null) return;
            _poolManager = poolManager;
        }

        public void BuffApply(BuffType buffKey, float value, int duration = 0)
        {
            BuffInfo buffInfo = new BuffInfo
            {
                BuffKey = (int)buffKey,
                Value = value
            };

            if (!_buffTable.ContainsKey((int)buffKey))
            {
                Debug.Log($"해당 버프 키를 가진 버프가 존재하지 않음. {buffKey}");
                return;
            }
            
            BuffVisualSO buffDataSo = _uiBuffTable[(int)buffKey];
            Entity target = uiAnchor;
            if (_activeBuffTable.TryGetValue(buffInfo , out var buff) && buff && Mathf.Approximately(buffInfo.Value, buff.Value))
            {
                buff = _activeBuffTable[buffInfo];
                buff.BuffInit(_owner, value, duration, parentRoot);
            }
            else
            {
                BuffDataSO buffData = _buffTable[(int)buffKey];

                buff = _poolManager.Pop<Buff>(buffData.buffPrefab);
                buff.BuffInit(_owner, value, duration, parentRoot);
                _activeBuffTable[buffInfo] = buff;
            }
            var op = BuffUiOp.Applied;
            if (buff != null && buff.IsActive) 
                op = BuffUiOp.Updated;
            
            Bus<BuffUiEvent>.Raise(new BuffUiEvent(target, buffDataSo, op, value, duration));
        }

        public void BuffApply(Elemental newElemental, int duration = 0)
        {
            if (newElemental == Elemental.None)
            {
                Debug.Log("속성이 존재하지 않아 무시됩니다.");
                return;
            }

            Debug.Log($"해당 속성이 적용됩니다. {newElemental}");
            _skillEnchantOverride.EnchantAction(newElemental, duration);
        }

        public void ImmediatelyBuff(Elemental newElemental)
        {
            if (newElemental == Elemental.None)
            {
                Debug.Log("속성이 존재하지 않아 무시됩니다.");
                return;
            }

            Debug.Log($"해당 속성이 적용됩니다. {newElemental}");
            _skillImmediatelyEnchantOverride.EnchantAction(newElemental, 2);
        }

        public void BuffRemover(BuffInfo buffInfo)
        {
            Debug.Log($"{buffInfo} 버프 제거");

            Buff buff = _activeBuffTable[buffInfo];
            if (buff != null && !buff.IsActive)
            {
                buff.OnBuffRemove();
                _activeBuffTable[buffInfo] = null;
            }

            BuffVisualSO buffDataSo = _uiBuffTable[buffInfo.BuffKey];
            ModuleOwner target = uiAnchor;

            Bus<BuffUiEvent>.Raise(new BuffUiEvent(target, buffDataSo, BuffUiOp.Removed, 0f, 0));
        }

        public void UpdateTime()
        {
            UpdateBuffTime();
            UpdateEnchant();
        }

        private void UpdateEnchant()
        {
            Debug.Assert(_skillEnchantOverride != null, $"{_owner}에 EnchantInfo가 존재하지않습니다.");
            if (_skillEnchantOverride.IsValid)
            {
                _skillEnchantOverride.ResetEnchantInfo();
                return;
            }

            bool isEnd = _skillEnchantOverride.IsEnd;

            if (isEnd)
            {
                _skillEnchantOverride.ResetEnchantInfo();
                return;
            }

            _skillEnchantOverride.UpdateEnchantInfo();
        }

        private void UpdateBuffTime()
        {
            ModuleOwner target = uiAnchor;

            foreach (var (key, buff) in _activeBuffTable)
            {
                if (buff == null) continue;

                if (!_uiBuffTable.TryGetValue(key.BuffKey, out var buffSO) || buffSO == null)
                    continue;

                int beforeTurn = buff.RemainingTurn;

                buff.UpdateBuffTime();

                bool isActive = buff.IsActive;
                int afterTurn = buff.RemainingTurn;

                if (isActive)
                {
                    if (afterTurn != beforeTurn)
                    {
                        Bus<BuffUiEvent>.Raise(new BuffUiEvent(
                            target, buffSO, BuffUiOp.Updated, 0f, afterTurn
                        ));
                    }

                    continue;
                }

                _disabledBuffBuffer.Add(key);
            }

            RefreshBuff();
        }

        private void RefreshBuff()
        {
            Debug.Log($"Buff Count : {_disabledBuffBuffer.Count}");
            if (_disabledBuffBuffer.Count <= 0) return;
            foreach (var key in _disabledBuffBuffer)
            {
                BuffRemover(key);
            }
            Debug.Log("Buff Clear");
            _disabledBuffBuffer.Clear();
        }

        public IReadOnlyList<(BuffVisualSO so, int remainingTurn)> GetActiveBuffs()
        {
            var result = new List<(BuffVisualSO, int)>();

            if (_buffTable == null || _uiBuffTable == null)
                return result;

            foreach (var (key, buff) in _activeBuffTable)
            {
                if (buff == null) continue;

                if (!buff.IsActive)
                    continue;

                if (!_uiBuffTable.TryGetValue(key.BuffKey, out var so) || so == null)
                    continue;

                result.Add((so, buff.RemainingTurn));
            }

            return result;
        }
        
        public IReadOnlyList<BuffInfo> GetRawActiveBuffs()
        {
            var result = new List<BuffInfo>();
            foreach (var kvp in _activeBuffTable)
            {
                if (kvp.Value != null && kvp.Value.IsActive)
                {
                    result.Add(kvp.Key);
                }
            }
            return result;
        }

#if UNITY_EDITOR
        public BuffType DebugElemental;
        public float value;
        
        [ContextMenu("Debug Elemental Invoke")]
        public void TestBuffInvoke()
        {
            BuffApply(DebugElemental, 1, 1);
        }

        [ContextMenu("Debug Elemental Remove")]
        public void TestBuffRemove()
        {
            BuffInfo buffInfo = new BuffInfo
            {
                BuffKey = (int)DebugElemental,
                Value = value
            };
            BuffRemover(buffInfo);
        }
#endif

        public bool TryGetElementalOverrideOrImmediately(out Elemental elemental)
        {
            Debug.Log($"속성 재정의 확인");
            if (_skillEnchantOverride != null && _skillEnchantOverride.IsValid)
            {
                Debug.Log($"인챈트 어트리뷰트 {_skillEnchantOverride.Elemental}");
                elemental = _skillEnchantOverride.Elemental;
                return true;
            }

            if (_skillImmediatelyEnchantOverride != null && _skillImmediatelyEnchantOverride.IsValid)
            {
                Debug.Log($"순간 인챈트 어트리뷰트 {_skillImmediatelyEnchantOverride.Elemental}");
                elemental = _skillImmediatelyEnchantOverride.Elemental;
                _skillImmediatelyEnchantOverride.ResetEnchantInfo();
                return true;
            }

            elemental = Elemental.None;
            return false;
        }
    }
}