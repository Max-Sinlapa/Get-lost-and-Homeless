using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckEnemyInFOVRang : Node
{
    //Target Layer
    private static int _enemyLayerMask = 1 << 3;

    private Transform _transform;
    private float _fovRange;
    
    private Animator _animator;
    private int _AnimWalk;

    public  CheckEnemyInFOVRang(Transform transform, float fovRange)
    {
        _transform = transform;
        _fovRange = fovRange;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("CheckEnemyInFOVRang");
        Transform target = (Transform)GetData("target");
        object t = GetData("target");

        //Debug.Log("Target =" + t);
        
        if (t == null)
        {
            //Debug.Log("NoTarget");
            
            /// CheckEnemy By Layer
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, _fovRange, _enemyLayerMask);

            if (colliders.Length > 0)
            {
                //Debug.Log("GOT Target");
                
                parent.parent.SetData("target", colliders[0].transform);
                //_animator.SetBool(_AnimWalk, true);
                
               // Debug.Log("Target =" + colliders[0]);
                
                state = NodeState.SUCCESS;
                return state;
            }

        }

        if (t != null && Vector3.Distance(_transform.position, target.position) > _fovRange)
        {
            //Debug.Log("Target Out Of Sight");
            state = NodeState.FAILURE;
            return state;
        }
        
        state = NodeState.SUCCESS;
        return state;
    }
}
