using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IceDEV
{
    public class SwapCharecterControl : MonoBehaviour
    {
        public GameObject player1;
        public GameObject player2;
        private GameObject activePlayer;

        void Start()
        {
            activePlayer = player1;
            player2.GetComponent<CharacterController>().enabled = false;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchCharacter();
            }
        }

        void SwitchCharacter()
        {
            if (activePlayer == player1)
            {
                activePlayer = player2;
                player1.GetComponent<CharacterController>().enabled = false;
                player2.GetComponent<CharacterController>().enabled = true;
            }
            else
            {
                activePlayer = player1;
                player1.GetComponent<CharacterController>().enabled = true;
                player2.GetComponent<CharacterController>().enabled = false;
            }
        }
    }
}

