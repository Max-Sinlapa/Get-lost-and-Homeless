using System;
using System.Collections;
using System.Collections.Generic;
using Max_DEV;
using UnityEngine;
using UnityEngine.Serialization;

public class AttackHitBox : MonoBehaviour
{
    public AttackController _attackController;
    public ObjectType_Identities _thisObjectType;
    private void OnTriggerEnter(Collider other)
    {
        ObjectType otherObjectType = other.GetComponent<ObjectType_Identities>().Type;
        
        /// Check Same ObjectType
        if(_thisObjectType.Type == otherObjectType)
        {
            Debug.Log("same tybe" );
            return;
        }
           
        
        var damage = other.GetComponent<HealthPoint>();
        Debug.Log("object=" + other);

        if (damage != null)
        {
            Debug.Log("object=" + other + " attackDamage= " + _attackController.attackDamage);
            if (_attackController == null)
                damage.DecreaseHp(0);
            else
                damage.DecreaseHp(_attackController.attackDamage);
        }
        
    }
}
