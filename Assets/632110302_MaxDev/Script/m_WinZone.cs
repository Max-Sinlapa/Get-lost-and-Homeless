using System;
using System.Collections;
using System.Collections.Generic;
using Max_DEV;
using Max_DEV.Interac;
using UnityEngine;
using UnityEngine.Events;

namespace Max_DEV
{
    public class m_WinZone : MonoBehaviour , IActorEnterExitHandler , IInteractable
    {
        public UnityEvent allPlayerInZone;

        private List<ObjectType> _listObjectType;
        private bool _CatIn = false;
        private bool _RatIn = false;

        private void Update()
        {
            
        }

        public void ActorEnter()
        {
            
            Debug.Log("Actor Enter");
            
            /*
            if (_CatIn && _RatIn)
            {
                allPlayerInZone.Invoke();
            }
            */
        }
        
        public void Interact()
        {
            throw new NotImplementedException();
        }
    
        public void ActorExit()
        {
            Debug.Log("Actor Exit");
        }
        
        private void OnTriggerEnter(Collider other)
        {
            ObjectType_Identities OtherType = other.GetComponent<ObjectType_Identities>();
            if (OtherType != null)
            {
                switch (OtherType.Type)
                {
                    case ObjectType.Cat:
                        _CatIn = true;
                        //_listObjectType.Add(OtherType.Type);
                        break;
                    case ObjectType.Mouse:
                        _RatIn = true;
                        //_listObjectType.Add(OtherType.Type);
                        break;
                }
            }
            
            if (_CatIn && _RatIn)
            {
                allPlayerInZone.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            ObjectType_Identities OtherType = other.GetComponent<ObjectType_Identities>();
            if (OtherType != null)
            {
                switch (OtherType.Type)
                {
                    case ObjectType.Cat:
                        _CatIn = false;
                        //_listObjectType.Remove(OtherType.Type);
                        break;
                    case ObjectType.Mouse:
                        _RatIn = false;
                        //_listObjectType.Remove(OtherType.Type);
                        break;
                }
            }
        }

        public void TestDubug()
        {
            Debug.Log("TEST DEBUG");
        }
        
    }
}
