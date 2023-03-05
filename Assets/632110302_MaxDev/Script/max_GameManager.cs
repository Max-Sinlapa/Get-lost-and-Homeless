using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Max_DEV
{
    public class max_GameManager : MonoBehaviour
    {
        public static int AllPlayerHealth;
        public static int PlayerScore;
        public int PlayerHealth;

        private void Awake()
        {
            AllPlayerHealth = PlayerHealth;
            PlayerScore = 0;
            
            Debug.Log("Player HP = " + AllPlayerHealth);
        }

        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
        
        
    }
}


