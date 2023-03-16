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
    public class m_GameManager : MonoBehaviour , IPunObservable , IOnEventCallback 
    {
        public static int _allPlayerCurrentHealth;
        public static int _playerCurrentScore;
        public static string _currentScene;

        public static int _startPlayerHealth;
        public static int _startPlayerScore;

        public Slider _HpSlider;
        
        private void Awake()
        {
            Debug.Log("Player HP = " + _allPlayerCurrentHealth);
            Debug.Log("Player Score = " + _playerCurrentScore);

            if (_HpSlider != null)
            {
                _HpSlider.maxValue = _allPlayerCurrentHealth;
            }
        }

        private void Update()
        {
            if (_HpSlider != null)
            {
                _HpSlider.value = _allPlayerCurrentHealth;
            }
        }

        [PunRPC]
        public void SetPlayerHealth(int _hp)
        {
            if (_HpSlider != null)
                _HpSlider.maxValue = _allPlayerCurrentHealth;
            
            _allPlayerCurrentHealth = _hp;
            Debug.Log("Player HP = " + _allPlayerCurrentHealth);
        }
        
        public static void SetPlayerScore(int _score)
        {
            _playerCurrentScore = _score;
        }



        #region Multiplayer MonoPUN
        
        /// <Rise Event : Reciver>
        public void OnEvent(EventData photonEvent)
        {
            Debug.Log("OnEvent-photonEventCode = " + photonEvent);
            
            if (photonEvent.Code == 10)
            {
                object[] data = (object[])photonEvent.CustomData;
                Debug.Log("HealthFormEventCode = " + (int)data[0]);
                SetPlayerHealth((int)data[0]);
            }
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


