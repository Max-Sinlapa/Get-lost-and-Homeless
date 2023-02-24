using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

namespace Max_DEV.Interac
{
    public class ActorTriggerHandler : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> m_TriggeredGameObjects = new();

        private void OnTriggerEnter(Collider other)
        {
            var interactableComponent = other.GetComponent<IInteractable>();
            if (interactableComponent != null)
            {
                if (interactableComponent is IActorEnterExitHandler enterExitHandler)
                {
                    enterExitHandler.ActorEnter();
                }

                m_TriggeredGameObjects.Add(other.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {

        }

        private void OnTriggerExit(Collider other)
        {
            var interactableComponent = other.GetComponent<IInteractable>();

            if (interactableComponent != null)
            {
                if (interactableComponent is IActorEnterExitHandler enterExitHandler)
                {
                    enterExitHandler.ActorExit();
                }

                m_TriggeredGameObjects.Remove(other.gameObject);
            }
        }

        public IInteractable GetInteractable()
        {
            if (m_TriggeredGameObjects.Count == 0)
            {
                return null;
            }
            
            return m_TriggeredGameObjects[0].GetComponent<IInteractable>();
        }
    }
}
