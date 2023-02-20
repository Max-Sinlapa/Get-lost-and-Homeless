using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using BehaviorTree;

public class TaskAttack : Node
{
    private Animator _animator;
    private int _AnimWalk;
    private int _AnimAttack;

    private Transform _lastTarget;
    private EnemyManager _enemyManager;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;
    
    public TaskAttack(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
        _AnimWalk = Animator.StringToHash("Walk");
        _AnimAttack = Animator.StringToHash("Attack");
    }

    public override NodeState Evaluate()
    {
        Debug.Log("TaskAttack");
        
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            _enemyManager = target.GetComponent<EnemyManager>();
            _lastTarget = target;
        }


        _attackCounter += Time.deltaTime;
        
        if (_attackCounter >= _attackTime)
        {
            bool enemyIsDead = _enemyManager.TakeHit();
            if (enemyIsDead)
            {
                ClearData("target");
                _animator.SetBool(_AnimAttack, false);
                _animator.SetBool(_AnimWalk, true);
            }
            else
            {
                _animator.SetBool(_AnimAttack, true);
                _animator.SetBool(_AnimWalk, false);
                _attackCounter = 0f;
            }
                
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}
