using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


namespace Max_DEV.Manager
{
    public class m_GameManager : MonoBehaviourPun , IPunObservable , IOnEventCallback 
    {
        public static string _currentScene;

        public static int _startPlayerHealth;
        public static int _startPlayerScore;
        
        public static int _allPlayerCurrentHealth = _startPlayerHealth;
        public static int _playerCurrentScore = _startPlayerScore;

        public Slider _HpSlider;
        

        private void Awake()
        {
            //Debug.Log("Player HP = " + _allPlayerCurrentHealth);
            //Debug.Log("Player Score = " + _playerCurrentScore);

            
            if (_HpSlider != null)
            {
                _HpSlider.maxValue = _allPlayerCurrentHealth;
            }

            _playerCurrentScore = _startPlayerScore;
            _allPlayerCurrentHealth = _startPlayerHealth;
            
            Debug.Log("manager Awake " + _startPlayerHealth);
            
        }
        

        private void Update()
        {
            if (_HpSlider != null)
            {
                if (_allPlayerCurrentHealth > _HpSlider.maxValue)
                {
                    _HpSlider.maxValue = _allPlayerCurrentHealth;
                }
                _HpSlider.value = _allPlayerCurrentHealth;
            }
        }

        [PunRPC]
        public static void Set_Start_PlayerHealth(int _hp)
        {
            _startPlayerHealth = _hp;
            Debug.Log("Player Start HP = " + _startPlayerHealth);

        }
        public static void SetPlayerHealth(int _hp)
        {
            _allPlayerCurrentHealth = _hp;
            Debug.Log("Player HP = " + _allPlayerCurrentHealth);
        }
        
        public static void Set_Start_PlayerScore(int _score)
        {
            _startPlayerScore = _score;
        }
        public static void SetPlayerScore(int _score)
        {
            _playerCurrentScore = _score;
        }
        


        #region Multiplayer MonoPUN
        
        /// <Rise Event : Reciver>
        public void OnEvent(EventData photonEvent)
        {
            //Debug.Log("OnEvent-photonEventCode = " + photonEvent);
        }
        
        public void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
            ///PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }
    
        public  void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            ///PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }
        
        /// </Rise Event : Reciver>
       

        /// <SerializeView>
        public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(_allPlayerCurrentHealth);
            }
            else {
                _allPlayerCurrentHealth = (int)stream.ReceiveNext();
            }
        }
        /// </SerializeView>
        
        #endregion
        
    }
}


