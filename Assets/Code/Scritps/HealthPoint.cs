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
    
        public bool ShereHP_inGameManager;
        
        [Header("Multiplayer")]
        public bool ShereHP_Multiplayer;
        
        [Header("Multiplayer-Serialization")]
        public bool Enemy_Hp_Serialization;
        public int Enemy_Hp;
        /*
        {
            get { return (int)_currentEnemy_HP; }
            set { _currentEnemy_HP = value; }
        }
        */

        //animator
        Animator animator;

        private void Start()
        {
    
            if (ShereHP_inGameManager)
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
            
            onHpChanged?.Invoke(currentHp);

            animator = GetComponent<Animator>();

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

            if (ShereHP_inGameManager)
            {
                m_GameManager._allPlayerCurrentHealth = currentHp;
                currentHp = m_GameManager._allPlayerCurrentHealth;
            }
        }
        
        [PunRPC]
        public void DecreaseHp(int _value) 
        {
            if (ShereHP_inGameManager)
            {
                
                currentHp = m_GameManager._allPlayerCurrentHealth;
                currentHp -= _value;
                m_GameManager.SetPlayerHealth(currentHp);
                Debug.Log("DecreaseManagerHealth = " + m_GameManager._allPlayerCurrentHealth);
                //onHpChanged?.Invoke(currentHp);
                
            }
            if (ShereHP_Multiplayer)
            {
                //Debug.Log("-------ShereHP_Multiplayer-------");
                //Debug.Log("11-currentHp = " + currentHp + " Damage = " + _value);
                Debug.Log("11-currentHp = " + currentHp);
                currentHp -= 1;
                Debug.Log("22-currentHp = " + currentHp);

                if (this.GetComponent<PhotonView>())
                {
                    Debug.Log("33-currentHp = " + currentHp);
                    Room_HealthChangeProperties(currentHp);
                    Debug.Log("Form"+ this.gameObject +" ViewID = " + photonView.ViewID);
                    Debug.Log("44-currentHp = " + currentHp);

                }
                HealthText();
                m_GameManager._allPlayerCurrentHealth = currentHp;
                Debug.Log("0000-currentHp = " + currentHp + " Damage = " + _value);
                //Debug.Log("-------ShereHP_Multiplayer-------");
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
            
            if (Enemy_Hp_Serialization)
            { 
                Debug.Log("-------DecreaseHP ELSE-------"); 
                Debug.Log("currentHp = " + currentHp + " Damage = " + _value);
                HealthText(); 
                currentHp -= _value; 
                Enemy_Hp = currentHp; 
                Debug.Log("-------DecreaseHP ELSE-------");
            }
            if (!Enemy_Hp_Serialization && !ShereHP_Multiplayer && !ShereHP_inGameManager)
            {
                Debug.Log("-------DecreaseHP BOX ELSE-------" + "form = " + this.gameObject);
                currentHp -= _value; 
            }

            
            /////////
            if (animator != null)
                animator.SetTrigger("HitReactionTrigger");
            
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
                
                //this.gameObject.SetActive(false);
                Destroy(this.gameObject ,0.5f);
            }
        }
        
        public void Respawn() 
        {
            Debug.Log("gameObject = " + this.gameObject + " RESPAWN");
            
            GetComponent<CharacterController>().enabled = false;
            //Debug.Log(" :) "+transform.position);
            //Debug.Log(" :( " + spawnPoint.position);
            transform.position = spawnPoint.position;
            //currentHp = MaxHp;
            
            if (ShereHP_inGameManager)
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
                { PunGameSetting.PLAYER_Current_LIVES, _amountHPchange }
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            Debug.Log("Send-Room-HealthUpdate : " + props[0] + "2 =" + props[1]);
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
            Debug.Log("OnRoomPropertiesUpdate");

        }
        
         public void GetPlayerHealthUpdate(Hashtable propertiesThatChanged) 
         {
             object HealthFromProps;
             if (propertiesThatChanged.TryGetValue(PunGameSetting.PLAYER_Current_LIVES, out HealthFromProps)) 
             { 
                 Debug.Log("Get-Room-HealthUpdate : " + (int)HealthFromProps + " Form = " + this.photonView.ViewID ); 
                 currentHp = PunGameSetting.GetPlayerHealth((int)HealthFromProps);
             }
             HealthText();
         }
        
        public override void OnPlayerPropertiesUpdate(Player target, Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            if (!this.GetComponent<PhotonView>())
                GetHealthUpdate(propertiesThatChanged);
            //Debug.Log("OnPlayerPropertiesUpdate");

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

