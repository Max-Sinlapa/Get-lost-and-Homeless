using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Photon.Pun;
using Max_DEV.Manager;
using Photon.Realtime;
using Unity.VisualScripting;
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
        
        [Header("Multiplayer")]
        public bool ShereHP_Multiplayer;
        public bool Enemy_Health;
        
        [Header("Multiplayer-Serialization")]
        public bool Enemy_Hp_Serialization;
        public int Enemy_Hp;
        /*
        {
            get { return (int)_currentEnemy_HP; }
            set { _currentEnemy_HP = value; }
        }
        */
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
                Room_HealthChangeProperties(m_GameManager._allPlayerCurrentHealth);
            }

            if (Enemy_Hp_Serialization)
            {
                Enemy_Hp = currentHp;
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
                Debug.Log("-------ShereHP_Multiplayer-------");
                Debug.Log("11-currentHp = " + currentHp + " Damage = " + _value);
                currentHp -= _value;
                if (this.GetComponent<PhotonView>())
                {
                    Room_HealthChangeProperties(currentHp);
                }
                HealthText();
                m_GameManager._allPlayerCurrentHealth = currentHp;
                Debug.Log("22-currentHp = " + currentHp + " Damage = " + _value);
                Debug.Log("-------ShereHP_Multiplayer-------");
            }
            /*
            if (Enemy_Health)
            {
                Debug.Log("-------Enemy_Health-------");
                Debug.Log("11-currentHp = " + currentHp + " Damage = " + _value);
                currentHp -= _value;
                Local_HealthChangeProperties(currentHp);
                HealthText();
                Debug.Log("22-currentHp = " + currentHp + " Damage = " + _value);
                Debug.Log("-------Enemy_Health-------");
            }
            */
            else if(!ShereHPinGameManager && !ShereHP_Multiplayer && !Enemy_Health)
            {
                if (Enemy_Hp_Serialization)
                {
                    Debug.Log("-------DecreaseHP ELSE-------");
                    Debug.Log("currentHp = " + currentHp + " Damage = " + _value);
                    HealthText();
                    currentHp -= _value;
                    Enemy_Hp = currentHp;
                    Debug.Log("-------DecreaseHP ELSE-------");
                }
                
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
            if (Enemy_Hp_Serialization)
            {
                if (stream.IsWriting) 
                {
                    stream.SendNext(Enemy_Hp);
                    HealthText();
                }
                else 
                {
                    Enemy_Hp = (int)stream.ReceiveNext();
                    HealthText();
                }
            }

            if (ShereHP_Multiplayer)
            {
                if (stream.IsWriting) 
                {
                    stream.SendNext(currentHp);
                    HealthText();
                }
                else 
                {
                    currentHp = (int)stream.ReceiveNext();
                    HealthText();
                }
            }
        }

        public static void Room_HealthChangeProperties(int _amountHPchange)
        {
            Hashtable props = new Hashtable
            {
                {PunGameSetting.PLAYER_Current_LIVES, _amountHPchange }
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            //Debug.Log("Send-Room-HealthUpdate : " + props);
        }
        public void Local_HealthChangeProperties(int _amountHPchange)
        {
            Hashtable props = new Hashtable
            {
                {PunGameSetting.EnemyHealth, _amountHPchange }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            //Debug.Log("Send-LocalPlayer-HealthUpdate : " + props);
        }
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            if (this.GetComponent<PhotonView>())
                GetPlayerHealthUpdate(propertiesThatChanged);
            //Debug.Log("OnRoomPropertiesUpdate");

        }
        public override void OnPlayerPropertiesUpdate(Player target, Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            if (!this.GetComponent<PhotonView>())
                GetHealthUpdate(propertiesThatChanged);
            //Debug.Log("OnPlayerPropertiesUpdate");

        }
        public void GetPlayerHealthUpdate(Hashtable propertiesThatChanged) 
        {
            object HealthFromProps;
            if (propertiesThatChanged.TryGetValue(PunGameSetting.PLAYER_Current_LIVES, out HealthFromProps)) {
                //Debug.Log("Get-Room-HealthUpdate : " + (int)HealthFromProps);
                currentHp = PunGameSetting.GetPlayerHealth((int)HealthFromProps);
            }
            HealthText();
        }
        public void GetHealthUpdate(Hashtable propertiesThatChanged) 
        {
            object HealthFromProps;
            if (propertiesThatChanged.TryGetValue(PunGameSetting.EnemyHealth, out HealthFromProps)) {
                //Debug.Log("Get-LocalPlayer-HealthUpdate : " + (int)HealthFromProps);
                currentHp = PunGameSetting.GetHealth((int)HealthFromProps);
            }
            HealthText();
        }

        #endregion
    }
}

