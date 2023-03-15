using Max_DEV.MoveMent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControl : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem Move_particle;
    public bool particIsPlay = false;

    
    private void Update()
    {

        if (particIsPlay == true)
        {
            MoveParticlePlay();
        }
        else if (particIsPlay == false)
        {
            MoveParticleStop();
        }
    }
    private void MoveParticlePlay()
    {
        Move_particle.Play();
    }
    private void MoveParticleStop()
    {
       // Move_particle.Stop();
       Move_particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
