using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Max_DEV.Manager
{
    public class m_GameManager : MonoBehaviour , IPunObservable
    {
        public static int _allPlayerCurrentHealth;
        public static int _playerCurrentScore;
        public static string _currentScene;

        public static int _startPlayerHealth;
        public static int _startPlayerScore;
        
        
        private void Awake()
        {
            Debug.Log("Player HP = " + _allPlayerCurrentHealth);
            Debug.Log("Player Score = " + _playerCurrentScore);
        }

        public static void SetPlayerHealth(int _hp)
        {
            _allPlayerCurrentHealth = _hp;
            Debug.Log("Player HP = " + _allPlayerCurrentHealth);
        }
        
        public static void SetPlayerScore(int _score)
        {
            _playerCurrentScore = _score;
        }
        
        public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(_allPlayerCurrentHealth);
            }
            else {
                _allPlayerCurrentHealth = (int)stream.ReceiveNext();
            }
        }

    }
}


