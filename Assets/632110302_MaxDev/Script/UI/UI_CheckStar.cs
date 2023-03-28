using System.Collections;
using System.Collections.Generic;
using Max_DEV.Manager;
using UnityEngine;

public class UI_CheckStar : MonoBehaviour
{
    public CompletedDTW _dotwween;

    public int _playerHP;
    void Start()
    {
        //_playerHP = m_GameManager._allPlayerCurrentHealth;

        switch (_playerHP)
        {
            case > 4:
                Debug.Log("3 Star");
                break;
            case > 2:
                _dotwween.star[0] = null;
                break;
            case <= 2:
                _dotwween.star[1] = null;
                _dotwween.star[2] = null;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
