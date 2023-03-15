using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckEnemyInAttackRange : Node
{
    private Transform _transform;
    private Animator _animator;
    private int _AnimWalk;
    private int _AnimAttack;
    
    private float _attackrange;

    public CheckEnemyInAttackRange(Transform transform, float attackRange)
    {
        _transform = transform;
        _attackrange = attackRange;
        _animator = transform.GetComponent<Animator>();
        
        _AnimWalk = Animator.StringToHash("Walk");
        _AnimAttack = Animator.StringToHash("Attack");
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("CheckEnemyInAttackRange");
        
        object t = GetData("target");
        if (t == null)
        {
            _animator.SetBool(_AnimAttack, false);
            //_animator.SetBool(_AnimWalk, true);
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;
        if (target == null)
        {
            //ClearData("target");
            _animator.SetBool(_AnimAttack, false);
            _animator.SetBool(_AnimWalk, true);
            state = NodeState.SUCCESS;
            return state;
        }
        if (Vector3.Distance(_transform.position, target.position) <= _attackrange)
        {
            _animator.SetBool(_AnimAttack, true);
            _animator.SetBool(_AnimWalk, false);
            state = NodeState.SUCCESS;
            return state;
        }
        
        _animator.SetBool(_AnimAttack, false);
        //_animator.SetBool(_AnimWalk, true);
        state = NodeState.FAILURE;
        return state;
    }
}
