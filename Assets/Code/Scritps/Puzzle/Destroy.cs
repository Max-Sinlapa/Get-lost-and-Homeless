using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PUN_Destroy : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
