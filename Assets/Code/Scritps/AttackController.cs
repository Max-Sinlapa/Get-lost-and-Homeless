using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [Header("Attack")]
    public int attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private GameObject meleeHitBox;
    public bool isAttacking;

    public void PerformAttack()
    {
        /*
        if (isAttacking)
        {
            return;
        }
        StartCoroutine(IEAttack());
        */
    }

    private IEnumerator IEAttack()
    {
        isAttacking = true;
        meleeHitBox.SetActive(true);
        yield return new WaitForSeconds(attackSpeed);
        meleeHitBox.SetActive(false);
        isAttacking = false;
    }
    
}
