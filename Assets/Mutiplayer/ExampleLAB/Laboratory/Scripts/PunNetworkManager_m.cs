using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Cinemachine;
using UnityEngine.InputSystem;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

    public class PunNetworkManager_m : ConnectAndJoinRandom
    {

        /// <summary>
        /// Implement Class "IOnEventCallback" To Use Rise Event
        /// </summary>
        public enum gamestate {
            None = 0,
            GameStart = 1,
            GamePlay = 2,
            GameOver = 3
        }
        public gamestate _currentGamestate = gamestate.GameStart;
        public gamestate CurrentGamestate {
            get { return _currentGamestate; }
            set {
                _currentGamestate = value;
                if (PhotonNetwork.CurrentRoom == null)
                    return;
                Hashtable props = new Hashtable { { PunGameSetting.GAMESTATE, _currentGamestate.ToString() } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            }
        }
        /// <summary>
        /// Create delegate Method
        /// </summary>
        public delegate void GameStartCallback();
        public static event GameStartCallback OnGameStart;
        public delegate void GameOverCallback();
        public static event GameOverCallback OnGameOver;
        
        
        public static PunNetworkManager_m singleton;
        public CinemachineVirtualCamera _vCam;
        public InputActionAsset _inputActions;
        
        [Header("Spawn Info")]
        [Tooltip("The prefab to use for representing the player")]
        public GameObject CatPlayerPrefab;
        public GameObject RatPlayerPrefab;
        public GameObject CatSpawnPosition;
        public GameObject RatSpawnPosition;
        //public GameObject RocketPrefab;
        
    
        public List<GameObjectData> gameObjectList = new List<GameObjectData>();
        public class GameObjectData
        {
            public int viewID;
            public Vector3 position;
        }
    
        // Raise Event
        // Custom Event 10: Used as "RandomCallAirDropEvent" event
        //private readonly byte RandomCallAirDropEvent = 10;
    
        /*
        #region RiseEvent
    
        ///IOnEventCallback class implement
        
            public void OnEvent(EventData photonEvent) 
            {
                byte eventCode = photonEvent.Code;
                    if (eventCode == RandomCallAirDropEvent)
                    {
                        Debug.Log("Call Resise Event is : " + eventCode.ToString());
                        Debug.Log("Data : " + photonEvent.CustomData.ToString());
                        object[] data = (object[])photonEvent.CustomData; 
                        Debug.Log("Data : " + data.ToString());
                        Debug.Log("Data : " + data.Length);
                        
                        Vector3[] position = (Vector3[])data[0];
                        GameObject localRocket = Instantiate(RocketPrefab); 
                        
                        // Random
                        //position.Length
                        //PhotonNetwork.PlayerList
                        
                        localRocket.transform.position = position[0]; 
                        
                    }
            }
            public override void OnEnable() {
                base.OnEnable();
                // Raise Event
                PhotonNetwork.AddCallbackTarget(this);
            }
            public override void OnDisable() {
                base.OnDisable();
                // Raise Event
                PhotonNetwork.RemoveCallbackTarget(this);
            }
            
            // RaiseEvent with Local GameObject.
            private void CallRaiseEvent()
            {
    
                GameObject[] ListPlayer = GameObject.FindGameObjectsWithTag("Player");
                Vector3[] positions = new Vector3[ListPlayer.Length];
                int[] viewIDs = new int[ListPlayer.Length];
                GameObjectData gameObjectData = new GameObjectData();
                for (int i = 0; i < ListPlayer.Length; i++)
                {
                    positions[i] = ListPlayer[i].GetComponent<Transform>().position;
                    viewIDs[i] = ListPlayer[i].GetComponent<PhotonView>().ViewID;
                    Debug.Log("View ID: " + viewIDs[i] + " Position: " + positions[i]);
    
                    gameObjectData.viewID = viewIDs[i];
                    gameObjectData.position = positions[i];
                }
                Debug.Log(" gameObjectData: " + gameObjectData.viewID + gameObjectData.position);
                
    
                object[] content = new object[] { positions };
                 // You would have to set the Receivers to All
                // in order to receive this event on the local client as well
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                    Receivers = ReceiverGroup.All };
                SendOptions sendOptions = new SendOptions {
                    Reliability = true,
                    Encrypt = true };
                PhotonNetwork.RaiseEvent(RandomCallAirDropEvent, content,
                    raiseEventOptions, sendOptions);
                Debug.Log("Call Raise Event.");
            }
    
        
        #endregion
        */
        
        private void GameStartSetting() {
            CurrentGamestate = gamestate.GamePlay;
        }
        private void Awake()
        {
            singleton = this;
            //Add Reference Method to Delegate Method
            OnGameStart += GameStartSetting;
            //When Connected from Launcher Scene
            if (PhotonNetwork.IsConnected)
            {
                this.AutoConnect = false;
                OnJoinedRoom();
                Debug.Log("PhotonNetwork.IsConnected = true");
            }
        }
        
        
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log("New Player. " + newPlayer.ToString());
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            if (PunUserNetControl.LocalPlayerInstance == null)
            {
                Debug.Log("We are Instantiating LocalPlayer from " + SceneManagerHelper.ActiveSceneName);
                PunNetworkManager_m.singleton.SpawnPlayer();
            }
            else
            {
                Debug.Log("Ignoring scene load for " + SceneManagerHelper.ActiveSceneName);
            }
    
        }
        public void SpawnPlayer()
        {
            int CatOrRat = SettingPlayerTeam(PhotonNetwork.LocalPlayer);
            if (CatOrRat != 1 && CatOrRat != 2)
            {
                Debug.Log("Can't Spawn No Selected Role");
                return;
            }
    
            if (CatOrRat == 1)
                PhotonNetwork.Instantiate(CatPlayerPrefab.name, CatSpawnPosition.transform.position, Quaternion.identity);
            if (CatOrRat == 2)
                PhotonNetwork.Instantiate(RatPlayerPrefab.name, RatSpawnPosition.transform.position, Quaternion.identity);
            /*
            if (CatOrRat == 1)
                PhotonNetwork.Instantiate(CatPlayerPrefab.name, CatSpawnPosition.transform.position, Quaternion.identity, 0);
            if (CatOrRat == 2)
                PhotonNetwork.Instantiate(RatPlayerPrefab.name, RatSpawnPosition.transform.position, Quaternion.identity, 0);
            */
            Debug.Log("This team" + CatOrRat );
    
            //Debug.Log("PunNetworkManager playerHP = " + _playerHealth);
            //HealthPoint.Room_HealthChangeProperties(_playerHealth);
    
            //Debug.Log("This = " + photonView.ViewID + " Team = " + CatOrRat);
            
            // we're in a room. spawn a character for the local player
            // it gets synced by using PhotonNetwork.Instantiate
        }
    
        private int SettingPlayerTeam(Player sender)
        {
            Photon.Pun.UtilityScripts.PhotonTeam _currentTeam = Photon.Pun.UtilityScripts.PhotonTeamExtensions.GetPhotonTeam(sender);
    
            Debug.Log("team setting = " + _currentTeam);
            
            if (_currentTeam != null)
            {
                int color = _currentTeam.Code;
                return color;
            }
            return 0;
        }
        
        public void gameStateUpdate(Hashtable propertiesThatChanged) {
            object gameStateFromProps;
            if (propertiesThatChanged.TryGetValue(PunGameSetting.GAMESTATE, out gameStateFromProps)) 
            {
                Debug.Log("GetStartTime Prop is : " + gameStateFromProps);
                _currentGamestate = (gamestate)Enum.Parse(typeof(gamestate), (string)gameStateFromProps);
            }
            if(_currentGamestate == gamestate.GameOver)
                OnGameOver();
        }
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            gameStateUpdate(propertiesThatChanged);
        }
        
        private void Update()
        {
            /*
            if (!PhotonNetwork.IsMasterClient)
                return;
            */
    
    
            switch(_currentGamestate)
            {
                case gamestate.GameStart:
                    OnGameStart();
                    break;
    
                case gamestate.GamePlay:
                    /*
                    Keyboard kboard = Keyboard.current;
                    if (kboard.rKey.wasPressedThisFrame)
                        if (kboard.rKey.keyCode == Key.R)
                            CallRaiseEvent();
                    */
                    break;
            }
        }

        #region GameNetworkStateManagement
    
        public void LeaveRoom()
        {
            Debug.Log("gamestate Prop is : " + _currentGamestate);
            PhotonNetwork.LeaveRoom();
        }
        /// <summary>
        /// Called when the local player left the room.
        /// We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
        }
    
        #endregion
    }


