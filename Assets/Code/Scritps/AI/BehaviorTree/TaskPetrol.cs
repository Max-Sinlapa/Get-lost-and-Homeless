using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class TaskPatrol : Node
{
    [SerializeField] protected NavMeshAgent _thisAgent;
    
    private Transform _transform;
    private Transform[] _waypoints;
    private Animator _animator;
    private int _AnimWalk;
    private int _AnimAttack;
    
    private float _petrolSpeed;

    private int _currentWaypointIndex = 0;

    private float _waitTime;
    private float _waitCounter = 0f;
    private bool _waiting = false;

    public TaskPatrol(Transform transform, Transform[] waypoints, float petrolSpeed, float waiteTime, NavMeshAgent agent)
    {
        _transform = transform;
        _waypoints = waypoints;
        _waitTime = waiteTime;
        _petrolSpeed = petrolSpeed;
        
        _animator = transform.GetComponent<Animator>();
        _AnimWalk = Animator.StringToHash("Walk");
        _AnimAttack = Animator.StringToHash("Attack");
        
        /// Setup NavMesh
        _thisAgent = agent;
        _thisAgent.updateRotation = true;
        _thisAgent.updatePosition = true;
        _thisAgent.speed = _petrolSpeed;
        
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("TaskPetrol");
        
        _animator.SetBool(_AnimAttack, false);
        
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _waiting = false;
                _animator.SetBool(_AnimWalk, true);
            }
               
        }
        else
        {
            Transform wp = _waypoints[_currentWaypointIndex];
            if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
            {
                _transform.position = wp.position;
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                _animator.SetBool(_AnimWalk, false);
            }
            else
            {
                _thisAgent.SetDestination(wp.position);
                /*
                _transform.position = Vector3.MoveTowards(_transform.position, wp.position, _petrolSpeed * Time.deltaTime);
                _transform.LookAt(wp.position);
                */
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
