using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AttackController : MonoBehaviourPun 
{
    [Header("Attack")]
    public int attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private GameObject meleeHitBox;
    public bool isAttacking;
    private int PhotonViewID;

    private void Start()
    {
        if (GetComponent<PhotonView>())
        {
            PhotonViewID = GetComponent<PhotonView>().ViewID;
        }
        
    }

    public void PerformAttack()
    {
        if (isAttacking)
        {
            return;
        }
        StartCoroutine(IEAttack());
    }
    
    
    public void PerformAttackRPC(int OwnerNetID)
    {
        Debug.Log("PerformAttackRPC Form = " + OwnerNetID );
        if (PhotonViewID == OwnerNetID)
        {
            photonView.RPC("AttackingByRPC", RpcTarget.All, OwnerNetID);
        }
    }
    
    [PunRPC]
    public void AttackingByRPC(int ownerHealth)
    {
        if (isAttacking)
        {
            return;
        }
        StartCoroutine(IEAttack());
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
