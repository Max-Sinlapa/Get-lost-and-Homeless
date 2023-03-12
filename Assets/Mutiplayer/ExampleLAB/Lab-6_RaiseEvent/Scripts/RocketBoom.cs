using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RocketBoom : MonoBehaviour
{
    public int Damage = 5;
    public LayerMask _playerMask;
    public int OwnerViewID = -1;
    private void OnCollisionEnter(Collision other) {
        Collider[] _player = Physics.OverlapSphere(this.transform.position,
            10, _playerMask);
        for (int i = 0; i < _player.Length; i++) {
            PunHealth otherHeal = _player[i].gameObject.
                GetComponent<PunHealth>();
            otherHeal.TakeDamage(Damage, OwnerViewID);
        }
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if(this.transform.position.y <= -10)
            Destroy(this.gameObject);
    }
}
