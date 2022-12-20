using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 direction;
    public float speed = 8;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float HorizonInput = Input.GetAxis("Horizontal");
        direction.x = HorizonInput * speed;

        controller.Move(direction * Time.deltaTime);
    }
}
