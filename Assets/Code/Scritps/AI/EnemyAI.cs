using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent thisAgent;
    [SerializeField] protected Transform m_Target = null;
    
    // animation IDs
    private Animator _animator;
    private int _animWalk;
    private int _animDie;
    private int _animAttack;

    [Header("EnemySettup")]
    public int MoveSpeed;
    void Start()
    {
        thisAgent = GetComponent<NavMeshAgent>();
        thisAgent.updateRotation = true;
        thisAgent.updatePosition = true;
        thisAgent.speed = MoveSpeed;
        
        _animator = GetComponent<Animator>();
        AssignAnimationIDs();
    }

    
    void Update()
    {
        Move();
    }

    private void AssignAnimationIDs()
    {
        _animWalk = Animator.StringToHash("Walk");
        _animDie = Animator.StringToHash("die");
        _animAttack = Animator.StringToHash("Attack");
        
    }
    
    public void SetTargetDestination(Transform target)
    {
        thisAgent.SetDestination(target.position);
        m_Target = target;
        
        Debug.Log("LockTarget");
    }

    public void ClearTarget()
    {
        m_Target = null;
    }
    
    public void Move()
    {
        if (m_Target != null)
        {
            if (transform.position.x - m_Target.position.x < 1 || transform.position.x - m_Target.position.x > -1)
            {
                if (transform.position.z - m_Target.position.z < 1 || transform.position.z - m_Target.position.z > -1)
                {
                    _animator.SetBool(_animWalk, false);
                    return;
                }
            }
                
            this.transform.position =
                Vector3.MoveTowards(transform.position, m_Target.position, MoveSpeed * Time.deltaTime);
            transform.LookAt(m_Target.position);
            _animator.SetBool(_animWalk, true);
            Debug.Log("CHASE");

        }
        else
        {
            _animator.SetBool(_animWalk, false);
            Debug.Log("NoTarget");
        }
    }
}
