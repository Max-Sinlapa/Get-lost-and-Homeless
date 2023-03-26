using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class platformTrigger : MonoBehaviour
{
    public platformMoveMent _platformAI;
    public GameObject _platform;
    
    public float StartSpeed;
    public float PetrolSpeed = 2f;
    
    public Transform[] _waypoints;
    private int _currentWaypointIndex = 0;


    public float _CheckDistant;

    public float currentDistanct;

    private bool trigger;
    
    void Start()
    {
        trigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        { 
            OnMove();
        }
        //OnMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("platform Enter");
        trigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("platform Exite");
        trigger = false;
    }

    private void OnMove()
    {

        Transform wp = _waypoints[_currentWaypointIndex];
        
        currentDistanct = Vector3.Distance(_platform.transform.position, wp.position);

        if (Vector3.Distance(_platform.transform.position, wp.position) < _CheckDistant)
        {
            _platform.transform.position = wp.position;

           // Debug.Log("_currentWaypointIndex = " + _currentWaypointIndex);

            if (_currentWaypointIndex == _waypoints.Length - 1)
            {
                _currentWaypointIndex = 0;
            }
            else
            {
                _currentWaypointIndex++;
                //_currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            }
        }
        else
        {
            _platform.transform.position = Vector3.MoveTowards(_platform.transform.position, wp.position, PetrolSpeed * Time.deltaTime);
        }
    }
}
