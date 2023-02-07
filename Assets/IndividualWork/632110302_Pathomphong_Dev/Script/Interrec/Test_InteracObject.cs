using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Max_DEV.Interac;
using Max_DEV;

namespace Max_DEV.Interac
{
   public class Test_InteracObject : MonoBehaviour , IInteractable , IActorEnterExitHandler
   {
       [SerializeField] protected TextMeshProUGUI m_InteractionTxt;
       [SerializeField] protected float m_Power = 10;
       private Rigidbody m_rigidBody;
       
       public UnityEvent onEnterEvent;
       public UnityEvent onInteracEvent;
       public UnityEvent onExiteEvent;
       
   
       private void Start()
       {
           m_rigidBody = GetComponent <Rigidbody>();
       }
   
       public void Interact()
       {
           m_rigidBody.AddForce(Vector3.up*m_Power,ForceMode.Impulse);
           
           onInteracEvent.Invoke();
       }
   
       public void ActorEnter()
       {
           //m_InteractionTxt.gameObject.SetActive(true);
           onEnterEvent.Invoke();
       }
   
       public void ActorExit()
       {
           onEnterEvent.Invoke();
           //m_InteractionTxt.gameObject.SetActive(false);
       }
   
       private void OnTriggerEnter(Collider other)
       {
           ObjectType_Identities OtherType = other.GetComponent<ObjectType_Identities>();
           if (OtherType != null)
           {
               switch (OtherType.Type)
               {
                   case ObjectType.Cat:
                       break;
                   case ObjectType.Mouse:
                       break;
               }
           }
       }
   } 
}

