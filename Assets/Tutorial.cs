using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IceDEV
{
    public class Tutorial : MonoBehaviour
    {
        public GameObject UI;
        private bool triggered = false;

        private void Awake()
        {
            triggered = false;
        }
        void OnTriggerEnter(Collider other)
        {
            if (!triggered)
            {
                if (other.gameObject.tag == "Player")
                {
                    UI.SetActive(true);
                    triggered = true;
                    Destroy(UI, 5f);

                }
            }
                
        }
        
    }
}

