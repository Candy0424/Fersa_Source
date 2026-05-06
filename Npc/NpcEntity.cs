using CIW.Code;
using CIW.Code.FSM;
using UnityEngine;
using Work.CSH.Scripts.Player.States;

namespace Work.YIS.Code.Npc
{
    public class NpcEntity : Entity
    {
        [SerializeField] protected StateListSO stateList;
        
        protected EntityStateMachine stateMachine;
        
        protected override void InitializeModules()
        {
            base.InitializeModules();
            stateMachine = new EntityStateMachine(this, stateList.states);
        }

        protected virtual void Update()
        {
            stateMachine.UpdateStateMachine();
        }
    }
}