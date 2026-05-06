using CIW.Code.FSM;
using Code.Scripts.Enemies;
using UnityEngine;
using Work.CSH.Scripts.FSMSystem;
using Work.CSH.Scripts.Player.States;

namespace Work.YIS.Code.Npc
{
    public class Dummy : FieldEnemy
    {
        [SerializeField] private StateListSO stateList;
        
        private EntityStateMachine _stateMachine;

        protected override void InitializeModules()
        {
            base.InitializeModules();
            _stateMachine = new EntityStateMachine(this, stateList.states);
        }

        protected override void Start()
        {
            base.Start();
            ChangeState(DummyState.IDLE);
        }

        public void ChangeState(DummyState newState) => _stateMachine.ChangeState((int)newState);
    }
}