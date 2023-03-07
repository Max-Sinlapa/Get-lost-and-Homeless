using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Max_DEV
{
    public class max_GameManager : MonoBehaviour
    {
        public static int _allPlayerHealth;
        public static int _playerScore;
        public int PlayerHealth;

        private void Awake()
        {
            _allPlayerHealth = PlayerHealth;
            _playerScore = 0;
            
            Debug.Log("Player HP = " + _allPlayerHealth);
        }

        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }

        public void AddPlayerScore(int _score)
        {
            _playerScore += _score;
        }
        
        
        
    }
}


