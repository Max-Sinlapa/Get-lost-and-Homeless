using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeath : MonoBehaviour
{
    // Start is called before the first frame update
    public static float currentHeath = 3;
    protected bool gameOver = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHeath <= 0)
        {
            gameOver = true;
        }
    }
}
