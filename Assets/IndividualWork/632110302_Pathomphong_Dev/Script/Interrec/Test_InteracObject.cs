using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Max_DEV.Interac;
using TMPro;

public class Test_InteracObject : MonoBehaviour , IInteractable , IActorEnterExitHandler
{
    [SerializeField] protected TextMeshProUGUI m_InteractionTxt;
    [SerializeField] protected float m_Power = 10;
    private Rigidbody m_rigidBody;

    private void Start()
    {
        m_rigidBody = GetComponent <Rigidbody>();
    }

    public void Interact()
    {
        m_rigidBody.AddForce(Vector3.up*m_Power,ForceMode.Impulse);
    }

    public void ActorEnter()
    {
        //m_InteractionTxt.gameObject.SetActive(true);
    }

    public void ActorExit()
    {
        //m_InteractionTxt.gameObject.SetActive(false);
    }
}
