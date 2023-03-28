using System.Collections;
using System.Collections.Generic;
using Max_DEV;
using UnityEngine;
using UnityEngine.Events;

public class SceneTrigger : MonoBehaviour
{

    public bool _CheckCat;
    public bool _CheckRat;

    public UnityEvent cutSceneTrigger;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("object = " + other.gameObject);

        
        ObjectType_Identities OtherType = other.GetComponent<ObjectType_Identities>();
        if (OtherType != null)
        {
            switch (OtherType.Type)
            {
                case ObjectType.Cat:
                    if (_CheckCat)
                    {
                        Debug.Log("SceneTrigger IN-WORK");
                        cutSceneTrigger.Invoke();
                    }
                    break;
                case ObjectType.Mouse:
                    if (_CheckRat)
                    {
                        cutSceneTrigger.Invoke();
                    }
                    break;
            }
        }
    }
}
