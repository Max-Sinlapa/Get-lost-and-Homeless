using System;
using System.Collections;
using System.Collections.Generic;
using Max_DEV;
using Max_DEV.Interac;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Max_DEV
{
    public class m_WinZone : MonoBehaviourPun , IActorEnterExitHandler , IInteractable , IPunObservable
    {
        [Header("Next-Scenes_Singleplayer")]
        public UnityEvent allPlayerInZone;

        [Header("Next-Scenes_Multiplayer")]
        public string _NextScene;
        
        
        private List<ObjectType> _listObjectType;
        private bool _CatIn = false;
        private bool _RatIn = false;

        private bool _AllPlayerIn = false;

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
                _AllPlayerIn = true;
                
                PhotonView other_photonView = PhotonView.Get(other);
                if (other_photonView != null && _AllPlayerIn)
                {
                    //other_photonView.RPC("InWokeChangeScene", RpcTarget.All, _NextScene);
                    InWokeChangeScene();
                    Debug.Log("RPC ChangScene");
                    _AllPlayerIn = false;
                }
                else
                {
                    Debug.Log("ChangScene NO RPC");
                    allPlayerInZone.Invoke();
                }
                
            }
        }

        
        [PunRPC]
        public void InWokeChangeScene()
        {
            //PhotonNetwork.LoadLevel(_NextScene);
            m_SceneManager.Multiplayer_ChangeScene(_NextScene);
            Debug.Log("InWokeChangeScene WAKE"); 
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

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            
            if (stream.IsWriting) 
            {
                stream.SendNext(_AllPlayerIn);
            }
            else 
            {
                _AllPlayerIn = (bool)stream.ReceiveNext();
            }
            
        }
    }
}
