using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using StarterAssets;


public class ShowUIText : MonoBehaviour
{
    public TextMeshProUGUI Uitext;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PunHealth playerHP = GetComponent<PunHealth>();
        Uitext.text = playerHP.currentHealth.ToString();
    }
}
