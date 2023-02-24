using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Max_DEV;

public class EnemyVision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ObjectType_Identities OtherType = other.GetComponent<ObjectType_Identities>();
        if (OtherType != null)
        {
            switch (OtherType.Type)
            {
                case ObjectType.Cat:
                    var targetPos = other.GetComponent<Transform>();
                    var enemy = GetComponentInParent<EnemyAI>();
                    enemy.SetTargetDestination(targetPos);
             
                    Debug.Log("Detected");
                    break;
                case ObjectType.Mouse:
                    break;
            }
        }
        
             
        Debug.Log("Detected");
        
    }

    private void OnTriggerExit(Collider other)
    {
        var enemy = GetComponentInParent<EnemyAI>();
        enemy.ClearTarget();
    }
}
