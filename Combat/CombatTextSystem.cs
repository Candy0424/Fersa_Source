using PSB_Lib.Dependencies;
using PSB_Lib.ObjectPool.RunTime;
using PSB.Code.BattleCode.Events;
using PSW.Code.EventBus;
using SW.Code.Battle;
using UnityEngine;
using YIS.Code.Defines;
using YIS.Code.Events;

namespace YIS.Code.Combat
{
    public class CombatTextSystem : MonoBehaviour
    {
        [Inject] private PoolManagerMono _poolManager;
        [SerializeField] private PoolItemSO damageTextPrefab;
        [SerializeField] private Vector2 dmgOffsetValue;

        private void Awake()
        {
            Bus<DmgTextUiEvent>.OnEvent += HandlePopUpText;
            Bus<HealTextUiEvent>.OnEvent += HandlePopUpHealText;
        }

        private void HandlePopUpHealText(HealTextUiEvent evt)
        {
            Vector2 pos = evt.TargetPos + dmgOffsetValue;
            float dmg = evt.Value;
            PopUpDamageText(pos, dmg);
        }

        private void PopUpDamageText(Vector2 pos, float damage)
        {
            _poolManager.Pop<DamageTextUI>(damageTextPrefab).PopUpText(pos, damage);
        }

        private void HandlePopUpText(DmgTextUiEvent evt)
        {
            Vector2 pos = evt.TargetPos + dmgOffsetValue;
            float dmg = evt.Value;
            Elemental eType = evt.ElementalType;
            
            PopUpDamageText(pos, dmg, eType);
        }

        private void PopUpDamageText(Vector2 pos, float damage, Elemental elementalType) => 
            _poolManager.Pop<DamageTextUI>(damageTextPrefab).PopUpText(pos, elementalType, damage);
    }
}