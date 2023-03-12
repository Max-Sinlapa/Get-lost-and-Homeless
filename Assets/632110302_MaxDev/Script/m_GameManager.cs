using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Max_DEV.Manager
{
    public class m_GameManager : MonoBehaviour 
    {
        public static int _allPlayerCurrentHealth;
        public static int _playerCurrentScore;
        public static string _currentScene;

        public static int _startPlayerHealth;
        public static int _startPlayerScore;
        
        
        private void Awake()
        {
            Debug.Log("Player HP = " + _allPlayerCurrentHealth);
            Debug.Log("Player Score = " + _playerCurrentScore);
        }

        public static void SetPlayerHealth(int _hp)
        {
            _allPlayerCurrentHealth = _hp;
        }
        
        public static void SetPlayerScore(int _score)
        {
            _playerCurrentScore = _score;
        }

    }
}


