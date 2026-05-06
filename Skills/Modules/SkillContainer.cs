using System;
using System.Collections.Generic;
using System.Linq;
using CIW.Code;
using PSB_Lib.Dependencies;
using PSB.Code.BattleCode.Events;
using PSB.Code.CoreSystem.Events;
using PSB.Code.CoreSystem.SaveSystem;
using PSW.Code.EventBus;
using UnityEngine;
using YIS.Code.Events;
using YIS.Code.Modules;

namespace YIS.Code.Skills.Modules
{
    [Serializable]
    public struct SkillDatas
    {
        public SkillDataSO[] Datas;
    }

    public class SkillContainer : MonoBehaviour, IModule, IAfterInitModule, ISaveable
    {
        private readonly Dictionary<string, SkillDataSO> _skills = new();

        private Entity _entity;
        private bool _isSubscribed;

        [field: SerializeField] public SaveId SaveId { get; private set; }
        
        [Inject] private ISaveStore _store;

        public void Initialize(ModuleOwner owner)
        {
            _entity = owner as Entity;
        }

        private void Start()
        {
            UpdateSkill();
        }

        public void AfterInitialize()
        {
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            if (_isSubscribed) return;

            Bus<ShopUIActiveEvent>.OnEvent += HandleUIActiveEvent;
            Bus<SpendSkillEvent>.OnEvent += HandleSpendSkillEvent;
            Bus<VillageResetEvent>.OnEvent += HandleVillageReset;
            
            _isSubscribed = true;
        }

        private void UnsubscribeEvents()
        {
            if (!_isSubscribed) return;

            Bus<ShopUIActiveEvent>.OnEvent -= HandleUIActiveEvent;
            Bus<SpendSkillEvent>.OnEvent -= HandleSpendSkillEvent;
            Bus<VillageResetEvent>.OnEvent -= HandleVillageReset;
            
            _isSubscribed = false;
        }
        
        private void HandleVillageReset(VillageResetEvent evt)
        {
            ClearAllSkills();
            Debug.Log("<color=purple>마을 초기화 - 획득한 스킬 리셋</color>");
        }

        private void HandleUIActiveEvent(ShopUIActiveEvent evt)
        {
            UpdateSkill();
        }

        private void HandleSpendSkillEvent(SpendSkillEvent evt)
        {
            if (evt.ShopSkill == null)
            {
                Debug.LogError("구매한 스킬 데이터가 null 입니다.");
                return;
            }

            if (!TryAddSkill(evt.ShopSkill))
            {
                Debug.Log("금액부족");
                return;
            }

            UpdateSkill();
            Debug.Log("금액충족");
        }

        private void UpdateSkill()
        {
            var validSkills = _skills.Values.Where(skill => skill != null).ToList();

            foreach (var skill in validSkills)
            {
                string skillName = skill.visualData != null
                    ? skill.visualData.uiName
                    : skill.skillName;

                Debug.Log($"스킬 {skillName}");
            }

            Bus<SkillUpdateEvent>.Raise(new SkillUpdateEvent(validSkills));
        }

        public bool TryAddSkill(SkillDataSO skill)
        {
            if (skill == null)
            {
                Debug.LogError("<color=red>스킬 추가 실패 : skill 이 null 입니다.</color>");
                return false;
            }

            if (string.IsNullOrWhiteSpace(skill.skillName))
            {
                Debug.LogError("<color=red>스킬 추가 실패 : skillName 이 비어있습니다.</color>");
                return false;
            }

            if (_skills.ContainsKey(skill.skillName))
            {
                Debug.LogWarning($"<color=yellow>스킬 추가 실패 : 이미 존재하는 스킬입니다. ({skill.skillName})</color>");
                return false;
            }

            _skills.Add(skill.skillName, skill);
            Debug.LogWarning($"<color=green>스킬 추가 성공 : {skill.skillName}</color>");
            return true;
        }

        #region SaveRegion

        public string GetSaveData()
        {
            SkillDataSO[] validSkills = _skills.Values
                .Where(skill => skill != null)
                .ToArray();

            var save = JsonUtility.ToJson(new SkillDatas
            {
                Datas = validSkills
            });

            return string.IsNullOrEmpty(save) ? "" : save;
        }

        public void RestoreSaveData(string saveData)
        {
            _skills.Clear();

            if (string.IsNullOrWhiteSpace(saveData))
            {
                return;
            }

            SkillDatas saveWrapper;

            try
            {
                saveWrapper = JsonUtility.FromJson<SkillDatas>(saveData);
            }
            catch (Exception e)
            {
                Debug.LogError($"RestoreSaveData : Json 파싱 중 예외 발생");
                return;
            }

            if (saveWrapper.Datas == null || saveWrapper.Datas.Length == 0)
            {
                return;
            }

            for (int i = 0; i < saveWrapper.Datas.Length; i++)
            {
                SkillDataSO skill = saveWrapper.Datas[i];

                if (skill == null || string.IsNullOrWhiteSpace(skill.skillName))
                {
                    continue;
                }

                if (_skills.ContainsKey(skill.skillName))
                {
                    continue;
                }

                _skills.Add(skill.skillName, skill);
            }

            UpdateSkill();
        }
        
        private void ClearAllSkills()
        {
            _skills.Clear();
            Bus<RequestSaveEvent>.Raise(new RequestSaveEvent());

            UpdateSkill();
        }

        #endregion
        
    }
}