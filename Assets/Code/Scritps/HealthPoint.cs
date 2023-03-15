using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Photon.Pun;
using Max_DEV.Manager;

namespace Max_DEV
{
    public class HealthPoint : MonoBehaviour , IPunObservable
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
    
        private void Start()
        {
            onHpChanged?.Invoke(currentHp);
    
            if (ShereHPinGameManager)
            {
                Debug.Log(""+this.gameObject+" : Health = " + m_GameManager._allPlayerCurrentHealth);
                currentHp = m_GameManager._allPlayerCurrentHealth;
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
            
        public void DecreaseHp(int _value) 
        {
            if (ShereHPinGameManager)
            {
                currentHp = m_GameManager._allPlayerCurrentHealth;
                currentHp -= _value;
                m_GameManager._allPlayerCurrentHealth = currentHp;
                Debug.Log("DecreaseManagerHealth = " + m_GameManager._allPlayerCurrentHealth);
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
        
        public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info) 
        {
            /*
            if (stream.IsWriting) {
                stream.SendNext(currentHp);
            }
            else {
                currentHp = (int)stream.ReceiveNext();
            }
            */
        }

    }
}

