using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class HealthPoint : MonoBehaviour
{
    [Header("Health")]
    public const int MaxHp = 5;
    [SerializeField] private int currentHp = MaxHp;
    public bool isDead = false;
    public TextMeshProUGUI playerHP_Text;
    [SerializeField] private Transform spawnPoint;
    public UnityEvent<int> onHpChanged;

    private void Start()
    {
        onHpChanged?.Invoke(currentHp);
    }

    public void HealthText()
    {
        if (playerHP_Text)
        {
            playerHP_Text.SetText(" "+ currentHp);
        }
    }
    
    public void Heal(int _value)
    {
        currentHp += _value;
        onHpChanged?.Invoke(currentHp);
    }
        
    public void DecreaseHp(int _value) 
    {
        currentHp -= _value;
        onHpChanged?.Invoke(currentHp);
        if (currentHp <= 0)
        {
            isDead = true;
            Debug.Log("Die");
            Death();
            
        }
        else
        {
            isDead = false;
        }
    }
    
    private void Death()
    {
        Respawn();
    }
    
    private void Respawn() 
    {
        GetComponent<CharacterController>().enabled = false;
        //Debug.Log(" :) "+transform.position);
        //Debug.Log(" :( " + spawnPoint.position);
        transform.position = spawnPoint.position;
        currentHp = MaxHp;
        GetComponent<CharacterController>().enabled = true;

    }
}
