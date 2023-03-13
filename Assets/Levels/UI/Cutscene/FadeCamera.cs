using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCamera : MonoBehaviour
{

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("FadeOut", 2);
    }

    // Update is called once per frame
    void Update()
    {
        animator.Play("FadeOut");
    }
}
