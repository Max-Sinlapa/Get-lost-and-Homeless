using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int _healthpoints;

    private void Awake()
    {
    }

    public bool TakeHit()
    {
        _healthpoints -= 10;
        bool isDead = _healthpoints <= 0;
        if (isDead) 
            _Die();
        return isDead;
    }

    private void _Die()
    {
        Destroy(gameObject);
    }
}
