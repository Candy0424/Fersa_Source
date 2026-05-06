using System.Collections.Generic;
using System.Linq;
using CIW.Code;
using Code.Scripts.Entities;
using PSB_Lib.StatSystem;
using UnityEngine;
using YIS.Code.Modules;

namespace YIS.Code.Combat
{
    public class EntityDamageCalcModule : MonoBehaviour, IModule
    {
        [SerializeField] private StatSO baseAtkStat;
        [SerializeField] private StatSO baseDefStat;
        [SerializeField] private StatListSO elementalAtkStatList;
        
        private Dictionary<string, StatSO> _elementalAtkStats;
        
        private Entity _entity;
        private EntityStat _stat;

        private const int DEFENSE_CONST_VALUE = 100; // 방어력 상수
        
        private float _damageMultiplier = 1; // 피해 증가량 - 기본 1
        private float _defenseMultiplier = 1; // 피해 감소량 - 기본 1
        
        public void Initialize(ModuleOwner owner)
        {
            _entity = owner as Entity;
            Debug.Assert(_entity != null, $"{owner}는 Entity가 아닙니다.");
            _stat = owner.GetModule<EntityStat>();
            Debug.Assert(_stat != null, $"{_entity.name}의 스탯 모듈이 없습니다. ");
        }

        public int DamageCalc(DamageData damageData, EntityStat targetStat, float simulatedAtkBuff = 0f)
        {
            float skillDamage = damageData.Damage;
            
            Debug.Assert(targetStat != null, $"공격 대상의 스탯모듈이 없습니다.");
            _stat.TryGetStat(baseAtkStat, out StatSO atkStat);
            targetStat.TryGetStat(baseDefStat, out StatSO defStat);
            
            Debug.Assert(atkStat != null && defStat != null,
                $"{_entity}의 공격력 스탯이나 방어력 스탯이 존재하지 않음 \n공격력 스탯 : {atkStat}\n방어력 스탯 : {defStat}");
            
            float atkValue = atkStat.Value + simulatedAtkBuff;
            float defValue = defStat.Value;

            float atkScaling = (atkValue / 100) + 1; // 공격력 계수
            float defScaling = DEFENSE_CONST_VALUE / (defValue + DEFENSE_CONST_VALUE); // 방어력 계수
            
            // 최종 데미지 = 스킬 공격력 * 공격력 계수 * 피해 증가량 계수 * 피해 감소량 계수 * 방어력 계수
            int totalDamage = Mathf.RoundToInt(skillDamage * atkScaling * _damageMultiplier * _defenseMultiplier * defScaling);
            
            return totalDamage;
        }
    }
}
