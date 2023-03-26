using System;
using System.Collections;
using System.Collections.Generic;
using Max_DEV;
using UnityEngine;

public class FloadBox : MonoBehaviour
{
    public float _floadPower;

    private Rigidbody m_rigid;
    
    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        ObjectType_Identities OtherType = other.GetComponent<ObjectType_Identities>();
        if (OtherType != null)
        {
            switch (OtherType.Type)
            {
                case ObjectType.Water:
                    floadUP();
                    break;
            }
        }
    }

    public void floadUP()
    {
        m_rigid.AddForce(0,_floadPower,0, ForceMode.Force);
    }
}
