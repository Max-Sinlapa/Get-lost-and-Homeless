using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Max_DEV.Interac;
using TMPro;
using UnityEngine.UI;

public class InteractableObjectWithTimer : MonoBehaviour , IInteractable , IActorEnterExitHandler
{
    
    [SerializeField] protected TextMeshProUGUI m_TextInfoEToInteract;
    [SerializeField] protected float m_TimerDuration = 5;
    
    [SerializeField] protected Slider m_SliderTimer;
    private bool _IsTimerStart = false;
    private float _StartTimeStamp;
    private float _EndTimeStamp;
    private float _SliderValue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_IsTimerStart) 
            return;
        
        _SliderValue = ((Time.time - _StartTimeStamp)/m_TimerDuration)*m_SliderTimer.maxValue;
        m_SliderTimer.value = _SliderValue;

        if (Time.time >= _EndTimeStamp)
        {
            _IsTimerStart = false;
        }
    }

    public void Interact()
    {
        StartTimer();
    }

    public void ActorEnter()
    {
        m_TextInfoEToInteract.gameObject.SetActive(true);
    }

    public void ActorExit()
    {
        m_TextInfoEToInteract.gameObject.SetActive(false);
    }

    private void StartTimer()
    {
        //Check if the timer is already running
        if (_IsTimerStart) 
            return;
        
        _IsTimerStart = true;
        _StartTimeStamp = Time.time;
        _EndTimeStamp = Time.time + m_TimerDuration;
        _SliderValue = 0;
    }
    
    
}
