using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Photon.Pun;
using Max_DEV.Manager;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Max_DEV
{
    public class HealthPoint : MonoBehaviourPunCallbacks , IPunObservable
    {
        public bool CanRespawn;
        [SerializeField] private Transform spawnPoint;
        
        [Header("Health")]
        public const int MaxHp = 5;
        [SerializeField] private int currentHp;
        public bool isDead = false;
        public TextMeshProUGUI playerHP_Text;
        public UnityEvent<int> onHpChanged;
    
        public bool ShereHPinGameManager;
        public bool ShereHP_Multiplayer;
    
        private void Start()
        {
            onHpChanged?.Invoke(currentHp);
    
            if (ShereHPinGameManager)
            {
                Debug.Log(""+this.gameObject+" : Health = " + m_GameManager._allPlayerCurrentHealth);
                currentHp = m_GameManager._allPlayerCurrentHealth;
            }
            
            
            if (this.GetComponent<PhotonView>())
            {
                HealthChangeProperties(101);
            }
        }
    
        public void HealthText()
        {
            if (playerHP_Text)
            {
                playerHP_Text.SetText(" "+ currentHp);
            }
        }
        
        public void Heal(int _value)
        {
            currentHp += _value;
            onHpChanged?.Invoke(currentHp);

            if (ShereHPinGameManager)
            {
                m_GameManager._allPlayerCurrentHealth = currentHp;
                currentHp = m_GameManager._allPlayerCurrentHealth;
            }
        }
        
        [PunRPC]
        public void DecreaseHp(int _value) 
        {
            
            if (ShereHPinGameManager)
            {
                currentHp = m_GameManager._allPlayerCurrentHealth;
                currentHp -= _value;
                m_GameManager._allPlayerCurrentHealth = currentHp;
                Debug.Log("DecreaseManagerHealth = " + m_GameManager._allPlayerCurrentHealth);
            }

            if (ShereHP_Multiplayer)
            {
                currentHp -= _value;
                if (this.GetComponent<PhotonView>())
                {
                    HealthChangeProperties(currentHp);
                }
            }
            else
            {
                currentHp -= _value;
            }   
            
            onHpChanged?.Invoke(currentHp);
            
            if (currentHp <= 0)
            {
                isDead = true;
                Debug.Log("Die");
                Death();
                
            }
            else
            {
                isDead = false;
            }
        }
        
        private void Death()
        {
            if(CanRespawn)
                Respawn();
            else
            {
                Destroy(this.gameObject);
            }
        }
        
        private void Respawn() 
        {
            GetComponent<CharacterController>().enabled = false;
            //Debug.Log(" :) "+transform.position);
            //Debug.Log(" :( " + spawnPoint.position);
            transform.position = spawnPoint.position;
            currentHp = MaxHp;
            
            if (ShereHPinGameManager)
                m_GameManager._allPlayerCurrentHealth = currentHp;

            GetComponent<CharacterController>().enabled = true;
        }

        #region MonoPUN

        public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info) 
        {
            if (stream.IsWriting) {
                stream.SendNext(m_GameManager._allPlayerCurrentHealth);
            }
            else {
                m_GameManager._allPlayerCurrentHealth = (int)stream.ReceiveNext();
            }
        }

        public void HealthChangeProperties(int _amountHPchange)
        {
            Hashtable props = new Hashtable
            {
                {PunGameSetting.PLAYER_Current_LIVES, _amountHPchange }
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            Debug.Log("Send-HealthUpdate : " + props);
        }
        
        
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            if (this.GetComponent<PhotonView>())
                GetHealthUpdate(propertiesThatChanged);
            Debug.Log("OnRoomPropertiesUpdate");

        }
        public void GetHealthUpdate(Hashtable propertiesThatChanged) 
        {
            object HealthFromProps;
            if (propertiesThatChanged.TryGetValue(PunGameSetting.PLAYER_Current_LIVES, out HealthFromProps)) {
                Debug.Log("Get-HealthUpdate : " + (int)HealthFromProps);
                currentHp = PunGameSetting.GetPlayerHealth((int)HealthFromProps);
            }
        }

        #endregion
    }
}

