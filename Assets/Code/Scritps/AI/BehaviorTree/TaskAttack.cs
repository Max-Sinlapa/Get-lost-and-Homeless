using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using Max_DEV;

namespace Max_DEV.Ai
{
    public class TaskAttack : Node
    {
        private Animator _animator;
        private int _AnimWalk;
        private int _AnimAttack;
    
        private Transform _lastTarget;
        private HealthPoint _enemyHealthPoint;
    
        private AttackController _thisAttackControll;

        public TaskAttack(Transform transform, AttackController attackController)
        {
            _thisAttackControll = attackController;
            
            _animator = transform.GetComponent<Animator>();
            _AnimWalk = Animator.StringToHash("Walk");
            _AnimAttack = Animator.StringToHash("Attack");
        }
    
        public override NodeState Evaluate()
        {
            //Debug.Log("TaskAttack");

            object t = GetData("target");
            Transform target = (Transform)GetData("target");
            //_enemyHealthPoint = (HealthPoint)t;
            

            if (target != _lastTarget)
            {
                _lastTarget = target;
            }

            
            if (_lastTarget == null)

            {
                ClearData("target");
                _animator.SetBool(_AnimAttack, false);
                _animator.SetBool(_AnimWalk, true);
            }
            else 
            {
                Debug.Log("EnemyPerformAttack");
                _thisAttackControll.PerformAttack();
                _animator.SetBool(_AnimAttack, true);
                _animator.SetBool(_AnimWalk, false);
            }
            
            
            state = NodeState.RUNNING;
            return state;
        }
    }
}


