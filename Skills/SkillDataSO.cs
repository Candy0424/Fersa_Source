using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Code.Scripts.Enemies.BT;
using PSB_Lib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Serialization;
using Work.PSB.Code.CoreSystem.Sounds;
using YIS.Code.Defines;
using YIS.Code.Items;

namespace YIS.Code.Skills
{
    public enum ChainDirection
    {
        Prev, Next
    }

    public enum ConditionerType
    {
        AND, OR
    }

    [CreateAssetMenu(fileName = "NewSkillItem", menuName = "SO/Skill/SkillData", order = 0)]
    public class SkillDataSO : ScriptableObject
    {
        [Header("Skill Attributes")] public Grade grade;
        public SkillType skillType;
        public BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        public Elemental elemental;

        [Header("Skill Data")] public string skillName; // 스킬 이름(이넘용)
        public ItemVisualDataSO visualData; // 스킬의 비주얼적 요소가 담긴 SO
        public SkillUpgradeDataSO upgradeData; // 스킬의 업그레이드 정보가 담긴 SO
        public int index;
        public string className; // 스킬 클래스 이름
        public GameObject skillPrefab; // 스킬 프리팹
        [Range(0, 5)] public int range; // 사정거리 (적 때릴 수 있는 수)
        public float damage; // 데미지 (딜)
        public int cooldownTurn; // 쿨타임 (턴)
        public int durationTurn; // 지속시간 (턴)
        public float impulsePower; // 스킬 임펄스 힘
        
        [Header("Vfx")]
        public PoolItemSO animatorEffect;  //이펙트 so
        public AnimParamSO effectParam;  //이펙트 파람 so
        public bool playEffectOnSelf;  //자기한테 플레이 하는 이펙트인가
        public bool playEffectOnFlip;  //플립해서 플레이 하는 이펙트인가
        public SoundSO attackSound;  // 공격 시 사운드

        [Header("Skill Chain Condition")] [HideInInspector]
        public string checkAttributeName;

        [HideInInspector] public CheckType checkSkillType;
        [HideInInspector] public ConditionerType conditionerType;

        private Func<BaseSkill?, bool> _skillChainCheckAction;

        [ContextMenu("TestText Chain Skill")]
        public void TestChainSkill()
        {
            string result = ChainSkill();
            Debug.Log($"<color=cyan>[Chain TestText]</color> {result}");
        }

        [ContextMenu("Force Reset Elemental")]
        public void ForceReset()
        {
            if (elemental == Elemental.None)
                elemental = Elemental.Normal;
            Debug.Log("Elemental Reset to Normal");
        }

        public bool? CanChainCheck(BaseSkill prevSkill)
        {
            if (_skillChainCheckAction == null)
            {
                string result = ChainSkill();
                Debug.Log($"<color=yellow>[SkillDataSO]</color> {skillName} 액션 재컴파일 시도: {result}");
            }

            return _skillChainCheckAction?.Invoke(prevSkill);
        }
        
        private void OnValidate()
        {   
            if (elemental == Elemental.None)
                elemental = Elemental.Normal;
            if (skillPrefab != null && !skillPrefab.TryGetComponent(out BaseSkill skill))
                Debug.LogError($"해당 스킬은 BaseSkill을 상속받지 않았습니다. {skillPrefab}");
        }

        public string ChainSkill()
        {
            ParameterExpression skillType = Expression.Parameter(typeof(BaseSkill), "skill");

            if (string.IsNullOrEmpty(checkAttributeName))
            {
                _skillChainCheckAction = Expression.Lambda<Func<BaseSkill, bool>>(Expression.Constant(true), skillType).Compile();
                return "조건 없음 (항상 True)";
            }

            string[] interfaceNames = checkAttributeName.Split(',').Select(s => s.Trim()).ToArray();
            Type[] interfaceTypes = GetInterfaceTypes(interfaceNames);

            try
            {
                Expression body = null;

                for (int i = 0; i < interfaceTypes.Length; i++)
                {
                    Type iType = interfaceTypes[i];

                    if (iType == null)
                        return $"{interfaceNames[i]}라는 이름을 가진 인터페이스는 없습니다.";

                    Expression checkExpr = Expression.TypeIs(skillType, iType);

                    if (body == null) body = checkExpr;
                    else body = (conditionerType == ConditionerType.OR) ? Expression.OrElse(body, checkExpr) : Expression.AndAlso(body, checkExpr);
                }

                if (body == null) return "유효한 인터페이스를 찾지 못함";

                _skillChainCheckAction = Expression.Lambda<Func<BaseSkill, bool>>(body, skillType).Compile();
                return "성공";
            }
            catch (Exception e)
            {
                return $"실패: {e.Message}";
            }
        }
        
        private Type GetInterfaceType(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .FirstOrDefault(p => p.IsInterface && p.Name == name);
        }

        private Type[] GetInterfaceTypes(params string[] names)
        {
            var allInterfaces = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes());

            return names.Select(tName => allInterfaces.FirstOrDefault(type => type.Name == tName && type.IsInterface)).ToArray();
        }

        // 이건 아직 사용하지 않는다.
        private Expression[] CheckSubClass(string inputParams)
        {
            string[] paramValues = inputParams.Split(',').Select(param => param.Trim()).ToArray();

            Expression[] args = new Expression[paramValues.Length];

            for (int i = 0; i < paramValues.Length; i++)
            {
                IEnumerable<Type> matchValue = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => p is { IsInterface: true } && p.Name == paramValues[i]);

                if (!matchValue.Any())
                    continue;

                var paramType = matchValue.First();
                args[i] = Expression.Constant(paramType);
            }

            return args;
        }
    }
}