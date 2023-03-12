using System;
using System.Collections;
using System.Collections.Generic;
using Max_DEV;
using UnityEngine;
using UnityEngine.Serialization;

public class AttackHitBox : MonoBehaviour
{
    public AttackController _attackController;
    public List<ObjectType> _FriendyObjectTypes;

    private bool sametype = false;
    private void OnTriggerEnter(Collider other)
    {
        ObjectType otherObjectType = other.GetComponent<ObjectType_Identities>().Type;
        ObjectType_Identities OtherType = other.GetComponent<ObjectType_Identities>();
        var damage = other.GetComponent<HealthPoint>();
        Debug.Log("object=" + other);
        
        /// Check Same ObjectType
        if (otherObjectType != null)
        {
            foreach (ObjectType friendObject in _FriendyObjectTypes)
            {

                if(friendObject == otherObjectType)
                {
                    sametype = true;
                    Debug.Log("same tybe" );
                    return;
                }
            }
            
            if (damage != null && !sametype)
            {
                Debug.Log("object=" + other + " attackDamage= " + _attackController.attackDamage);
                if (_attackController == null) 
                    damage.DecreaseHp(0);
                else
                    damage.DecreaseHp(_attackController.attackDamage);

                sametype = false;
            }
        }
        sametype = false;
    }
}
