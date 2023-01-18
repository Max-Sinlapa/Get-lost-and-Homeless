using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IceDEV
{
    public class PlayerCheckPoint : MonoBehaviour
    {
        // Start is called before the first frame update
        private Vector3 respawnPoint;
        protected Key RespawnKey = Key.R;
        void Start()
        {
            respawnPoint = this.gameObject.transform.position;
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard[RespawnKey].isPressed)
            {
                this.gameObject.transform.position = respawnPoint;
            }
        }

        // Update is called once per frame
        private void OnCollisionEnter(Collision target)
        {
            if (target.gameObject.tag == "CheckPoint")
            {
                respawnPoint = this.gameObject.transform.position;
                Debug.Log(respawnPoint);
            }
        }

    }
}

