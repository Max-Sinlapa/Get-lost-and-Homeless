using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReation : MonoBehaviour
{
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitReaction()
    {
        //_animator.SetTrigger();
    }
}
