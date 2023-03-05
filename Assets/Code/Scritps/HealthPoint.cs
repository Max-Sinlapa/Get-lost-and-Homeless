using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Max_DEV
{
    public class HealthPoint : MonoBehaviour
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
                Debug.Log("AllPlayerHealth = " + max_GameManager.AllPlayerHealth);
                currentHp = max_GameManager.AllPlayerHealth;
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
                max_GameManager.AllPlayerHealth = currentHp;
        }
            
        public void DecreaseHp(int _value) 
        {
            currentHp -= _value;

            if (ShereHPinGameManager)
            {
                max_GameManager.AllPlayerHealth = currentHp;
                Debug.Log("DecreaseManagerHealth" + max_GameManager.AllPlayerHealth);
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
        }
        
        private void Respawn() 
        {
            GetComponent<CharacterController>().enabled = false;
            //Debug.Log(" :) "+transform.position);
            //Debug.Log(" :( " + spawnPoint.position);
            transform.position = spawnPoint.position;
            currentHp = MaxHp;
            
            if (ShereHPinGameManager)
                max_GameManager.AllPlayerHealth = currentHp;

            GetComponent<CharacterController>().enabled = true;
    
        }
    }
}

