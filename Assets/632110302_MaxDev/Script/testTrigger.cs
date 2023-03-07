using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
       Debug.Log("" + this.gameObject + "Trigger = Stay");
       Debug.Log("By" + other.gameObject);
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("" + this.gameObject + "Trigger = Enter");
        Debug.Log("By" + other.gameObject);
    }
    
    void OnTriggerExit(Collider other)
    {
        Debug.Log("" + this.gameObject + "Trigger = Exite");
        Debug.Log("By" + other.gameObject);
    }
}
