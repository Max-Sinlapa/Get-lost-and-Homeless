using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(PhotonRigidbodyView))]

public class PunBullet : PunBaseInstance { 
    [Range(1, 10)]
    public int m_AmountDamage = 5;

    public float BulletForce = 20f;

    protected override void PunInstantiateObject(PhotonMessageInfo info)
    {
        base.PunInstantiateObject(info);

        //info.sender.TagObject = this.GameObject;
        Rigidbody bullet = GetComponent<Rigidbody>();
        // Add velocity to the bullet
        bullet.velocity = bullet.transform.forward * BulletForce;

        if (!photonView.IsMine)
            return;

        // Destroy the bullet after 10 seconds
        Destroy(this.gameObject, 10.0f);
    }
    
    protected override void TriggerWithPlayer(Collider other) {
        base.TriggerWithPlayer(other);

        PunHealth tempHealthOther = other.gameObject.GetComponent<PunHealth>();
        if (tempHealthOther != null)
            tempHealthOther.TakeDamage(m_AmountDamage, OwnerViewID);
        else Debug.Log("Empty Component.");
    }

}