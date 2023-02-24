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
    private void OnTriggerEnter2D(Collider2D other)
    {
        ObjectType otherObjectType = other.GetComponent<ObjectType_Identities>().Type;
        
        /// Check Same ObjectType
        if(_thisObjectType.Type == otherObjectType)
            return;
        
        var damage = other.GetComponent<HealthPoint>();
        
        if (damage != null)
        {
            if (_attackController == null)
                damage.DecreaseHp(0);
            else
                damage.DecreaseHp(_attackController.attackDamage);
        }
        
    }
}
