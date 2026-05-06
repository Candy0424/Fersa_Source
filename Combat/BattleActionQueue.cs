using System.Collections.Generic;
using System.Threading.Tasks;
using PSB_Lib.Dependencies;
using PSW.Code.EventBus;
using UnityEngine;
using YIS.Code.Events;
using YIS.Code.Skills.Sequences;

namespace YIS.Code.Combat
{
    public class BattleActionQueue : MonoBehaviour, IDependencyProvider
    {
        private readonly Queue<IBattleAction> _battleQueue = new();

        [Provide]
        private BattleActionQueue Provide() => this;

        public void EnBattleQueue(IBattleAction action)
        {
            if (action == null) return;
            
            _battleQueue.Enqueue(action);
            BattleAction();
        }

        public void BattleAction()
        {
            if (_battleQueue.Count > 0)
                _ = BattleActionExecute();
        }

        private async Task BattleActionExecute()
        {
            if (_battleQueue.Count == 0) return;
            
            while (_battleQueue.Count > 0)
            {
                var action = _battleQueue.Dequeue();
                if (action == null) continue;

                await action.ExecuteAsync();
                Debug.Log("큐 스택 실행");
            }
            
        }
    }
}