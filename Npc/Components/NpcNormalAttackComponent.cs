using System;
using Code.Scripts.Enemies;
using UnityEngine;
using Work.PSB.Code.FieldCode;
using YIS.Code.Modules;

namespace Work.YIS.Code.Npc.Components
{
    public class NpcNormalAttackComponent : MonoBehaviour, IModule, IAttackModule, IAfterInitModule
    {
        [Header("Attack Setting")]
        [SerializeField] private Vector2 attackRange;
        [SerializeField] private Vector2 offsetXY;
        [SerializeField] private LayerMask whatIsEnemy;
        
        public ModuleOwner Owner { get; private set; }
        
        public event Action OnAttackEnd;
        
        public void Initialize(ModuleOwner owner)
        {
            Owner = owner;
        }
        
        public void AfterInitialize()
        {
            
        }
        
        public void Attack(GameObject target = null)
        {
            Collider2D targetCol =
                Physics2D.OverlapBox((Vector2)transform.position + offsetXY, 
                    attackRange, 0, whatIsEnemy);
            
            if (targetCol == null) return;

            if (targetCol.TryGetComponent(out FieldEnemyData enemy))
            {
                Debug.Log("히트");
                enemy.Hit();
            }
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube((Vector2)transform.position + offsetXY, attackRange);
            Gizmos.color = Color.white;
        }
        #endif
    }
}