using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace IceDEV
{
    public class SwapCharecterControl : MonoBehaviour
    {
        [Header("Player")]
        public GameObject player1;
        public GameObject player2;
        private GameObject activePlayer;

        [Header("Camera")]
        public GameObject cam1;
        public GameObject cam2;
        private GameObject activeCam;

        

        public string idleAnimationTrigger = "Idle";

        void Start()
        {
            activePlayer = player1;
            if (activePlayer == player1)
            {
                //player2.GetComponent<CharacterController>().enabled = false;
                player2.GetComponent<Animator>().SetTrigger(idleAnimationTrigger);
                player2.GetComponent<PlayerInput>().enabled = false;

                activeCam = cam1;
                cam2.active = false;
            }
           

            
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchCharacter();
                SwitchCamera();
            }
        }

        void SwitchCharacter()
        {
            if (activePlayer == player2)
            {
                activePlayer = player1;

                //player1.GetComponent<CharacterController>().enabled = true;
               // player2.GetComponent<CharacterController>().enabled = false;
               

                player1.GetComponent<PlayerInput>().enabled = true;
                player2.GetComponent<PlayerInput>().enabled = false;
                

                player1.GetComponent<Animator>().ResetTrigger(idleAnimationTrigger);
                player2.GetComponent<Animator>().SetTrigger(idleAnimationTrigger);

               
                


            }
            else if (activePlayer == player1)
            {
                activePlayer = player2;

                //player1.GetComponent<CharacterController>().enabled = false;
                //player2.GetComponent<CharacterController>().enabled = true;
                

                player2.GetComponent<Animator>().ResetTrigger(idleAnimationTrigger);
                player1.GetComponent<Animator>().SetTrigger(idleAnimationTrigger);

                player2.GetComponent<PlayerInput>().enabled = true;
                player1.GetComponent<PlayerInput>().enabled = false;

            }
        }

        void SwitchCamera()
        {
            if (activeCam == cam1)
            {
                activeCam = cam2;
                cam1.active = false;
                cam2.active = true;
            }
            else
            {
                activeCam = cam1;
                cam1.active = true;
                cam2.active = false;
            }
        }
    }
}

