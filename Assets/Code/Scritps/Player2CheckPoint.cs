using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IceDEV
{
    public class Player2CheckPoint : MonoBehaviour
    {
        private Vector3 respawnPoint;
        protected Key RespawnKey = Key.T;
        void Start()
        {
            respawnPoint = transform.position;
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard[RespawnKey].isPressed)
            {
                transform.position = respawnPoint;
            }
        }

        // Update is called once per frame
        private void OnCollisionEnter(Collision target)
        {
            if (target.gameObject.tag == "CheckPoint")
            {
                respawnPoint = transform.position;
                Debug.Log(respawnPoint);
            }
        }
    }
}
