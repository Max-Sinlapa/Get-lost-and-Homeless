using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IceDEV
{
    public class PlayerContoller : MonoBehaviour
    {

        private Vector3 direction;
        public float speed = 8;
        protected Key ForwardKey = Key.W;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard[ForwardKey].isPressed)
            {
                this.transform.Translate(speed * Time.deltaTime, 0, 0);
            }




        }
    }
}