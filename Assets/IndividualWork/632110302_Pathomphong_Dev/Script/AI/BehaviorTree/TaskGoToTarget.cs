using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class TaskGoToTarget : Node
{
    [SerializeField] protected NavMeshAgent _thisAgent;
    
    private Transform _transform;

    private float _speedToTarget;
    
    private Animator _animator;
    private int _AnimWalk;
    private int _AnimAttack;

    public TaskGoToTarget(Transform transform, float speed, NavMeshAgent agent)
    {
        _transform = transform;
        _speedToTarget = speed;
        
        _animator = transform.GetComponent<Animator>();
        _AnimWalk = Animator.StringToHash("Walk");
        _AnimAttack = Animator.StringToHash("Attack");

        /// Setup NavMesh
        _thisAgent = agent;
        _thisAgent.updateRotation = true;
        _thisAgent.updatePosition = true;
        _thisAgent.speed = _speedToTarget;
        
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("TaskGoToTarget");
        
        Transform target = (Transform)GetData("target");

        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (Vector3.Distance(_transform.position, target.position) >= 0.01f)
        {
            _thisAgent.SetDestination(target.position);
            /*
            _transform.position = Vector3.MoveTowards(
                _transform.position, target.position, _speedToTarget * Time.deltaTime);
            
            _transform.LookAt(target.position);
            */
        }
        
        _animator.SetBool(_AnimAttack, false);
        _animator.SetBool(_AnimWalk, true);
        

        state = NodeState.RUNNING;
        return state;
    }
}
