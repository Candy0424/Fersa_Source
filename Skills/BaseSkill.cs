using System.Collections.Generic;
using CIW.Code;
using PSB.Code.BattleCode.Skills;
using PSW.Code.Dial.Slot;
using PSW.Code.EventBus;
using UnityEngine;
using YIS.Code.CoreSystem;
using YIS.Code.Defines;
using YIS.Code.Skills.Interfaces;
using YIS.Code.Skills.Modules;
using YIS.Code.Skills.Sequences;

namespace YIS.Code.Skills
{
    public abstract class BaseSkill : MonoBehaviour, ISkill, IElementalable
    {
        public SkillDataSO SkillData { get; private set; }

        public int Cooldown => SkillData.cooldownTurn;

        public int CurrentCooldown { get; protected set; }

        public ElementalState CurrentElementalState { get; private set; }
        
        public bool SkipDamageThisCast { get; protected set; }

        public void SetData(SkillDataSO data) => SkillData = data;
    
        public void Initialize()
        {
            CurrentElementalState ??= new ElementalState(SkillData.elemental);
            CurrentElementalState?.ResetElemental();
            CurrentCooldown = 0;
        }

        public bool CheckCooldown()
        {
            if (CurrentCooldown > 0)
            {
                Debug.Log($"현재 쿨타임. 남은 쿨 : {CurrentCooldown} / 맥스 쿨 {Cooldown}");
                Bus<CoolTimeEvemt>.Raise(new CoolTimeEvemt(SkillData, CurrentCooldown, Cooldown, false));
                return false;
            }

            return true;
        }

        public virtual IReadOnlyList<ISkillAction> GenerateSkill(bool isChain, Entity user, IReadOnlyList<Entity> targets)
        {
            SkipDamageThisCast = false;

            IReadOnlyList<ISkillAction> actions;

            if (isChain) actions = ChainSkillGenerateAction(user, targets);
            else actions = NormalSkillGenerateAction(user, targets);
            
            #if UNITY_EDITOR
            CurrentCooldown = Cooldown;
            #else
            CurrentCooldown = Cooldown + 1;
            #endif
            
            Debug.Log($"스킬 사용. 쿨타임 세팅 : {CurrentCooldown}");
            Bus<CoolTimeEvemt>.Raise(new CoolTimeEvemt(SkillData, CurrentCooldown, Cooldown, true));
            return actions;
        }
        
        public IReadOnlyList<ISkillAction> SimulateSkill(bool isChain, Entity user, IReadOnlyList<Entity> targets)
        {
            if (isChain) 
                return ChainSkillGenerateAction(user, targets);
            else 
                return NormalSkillGenerateAction(user, targets);
        }

        public void TickCooldown()
        {
            if (CurrentCooldown <= 0) return;

            CurrentCooldown--;
            Bus<CoolTimeEvemt>.Raise(new CoolTimeEvemt(SkillData, CurrentCooldown, Cooldown, false));
        }

        public void ElementalCal()
        {
            switch (CurrentElementalState.CurrentElemental)
            {
                case Elemental.None:
                default:
                    break;
                case Elemental.Normal:
                    OnElementalNormal();
                    break;
                case Elemental.Pyro:
                    OnElementalPyro();
                    break;
                case Elemental.Hydro:
                    OnElementalHydro();
                    break;
                case Elemental.Anemo:
                    OnElementalAnemo();
                    break;
                case Elemental.Cryo:
                    OnElementalCryo();
                    break;
                case Elemental.Electro:
                    OnElementalElectro();
                    break;
                case Elemental.Dendro:
                    OnElementalDendro();
                    break;
            }
        }

        protected virtual void OnElementalNormal() { }
        protected virtual void OnElementalPyro() { }
        protected virtual void OnElementalHydro() { }
        protected virtual void OnElementalAnemo() { }
        protected virtual void OnElementalCryo() { }
        protected virtual void OnElementalElectro() { }
        protected virtual void OnElementalDendro() { }

        public virtual bool CanChainPrev(BaseSkill prevSkill) => CheckInterface(prevSkill);
        public virtual bool CanChainNext(BaseSkill nextSkill) => CheckInterface(nextSkill);

        protected bool CheckInterface(BaseSkill skill)
        {
            bool? check = SkillData?.CanChainCheck(skill);
            return check is true;
        }

        public virtual float GetFinalDamage()
        {
            return SkillData != null ? SkillData.damage : 0f;
        }

        protected abstract IReadOnlyList<ISkillAction> NormalSkillGenerateAction(Entity user, IReadOnlyList<Entity> targets);
        protected abstract IReadOnlyList<ISkillAction> ChainSkillGenerateAction(Entity user, IReadOnlyList<Entity> targets);

        public void ChangeElemental(Elemental elemental) => CurrentElementalState.SetElemental(elemental);

        public IReadOnlyList<ISkillAction> ExecuteNormal(Context context, List<Entity> targets) => GenerateSkill(false, context.Caster, targets);
        public IReadOnlyList<ISkillAction> ExecuteChain(Context context, List<Entity> targets) => GenerateSkill(true, context.Caster, targets);
        
        public virtual bool SkipAnim(bool isChain)
        {
            return false;
        }
        
    }
}
