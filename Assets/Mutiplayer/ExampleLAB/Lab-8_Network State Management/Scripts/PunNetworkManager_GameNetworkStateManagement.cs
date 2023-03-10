using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Cinemachine;
using UnityEngine.InputSystem;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;


public class PunNetworkManager_GameNetworkStateManagement : ConnectAndJoinRandom, IOnEventCallback 
{

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
    
    
    public static PunNetworkManager_GameNetworkStateManagement singleton;
    public CinemachineVirtualCamera _vCam;
    public InputActionAsset _inputActions;
    
    [Header("Spawn Info")]
    [Tooltip("The prefab to use for representing the player")]
    public GameObject GamePlayerPrefab;
    public GameObject RocketPrefab;

    public List<GameObjectData> gameObjectList = new List<GameObjectData>();
    public class GameObjectData
    {
        public int viewID;
        public Vector3 position;
    }

    // Raise Event
    // Custom Event 10: Used as "RandomCallAirDropEvent" event
    private readonly byte RandomCallAirDropEvent = 10;

    #region RiseEvent

    ///IOnEventCallback class implement
        public void OnEvent(EventData photonEvent) {
        
            //Debug.Log(photonEvent.ToStringFull());
            byte eventCode = photonEvent.Code;
            //GameObject player = GameObject.FindGameObjectWithTag("Player");
           // Vector3 playertranforme = player.transform.position;
            if (eventCode == RandomCallAirDropEvent)
            {
                Debug.Log("Call Resise Event is : " + eventCode.ToString());
                Debug.Log("Data : " + photonEvent.CustomData.ToString());
                object[] data = (object[])photonEvent.CustomData; 
                Debug.Log("Data : " + data.ToString());
                Debug.Log("Data : " + data.Length);
                
                Vector3[] position = (Vector3[])data[0]; 
                //int color = (int)data[1];
                //Debug.Log("Position : " + position); Debug.Log("Color : " + color);
                // Instance Local Object
                GameObject localRocket = Instantiate(RocketPrefab); 
                
                // Random
                //position.Length
                //PhotonNetwork.PlayerList
                
               // Color currentColor = PunGameSetting.GetColor(color);
                localRocket.transform.position = position[0]; 
                //localRocket.GetComponent<RocketBoom>().Damage *= color;
                //localRocket.GetComponent<MeshRenderer>().material.color = currentColor;
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
        // Array contains the target position and the IDs of the selected units
        /*
        GameObject[] ListPlayer = GameObject.FindGameObjectsWithTag("Player");
        object[] content = new object[ListPlayer.Length];
        
         for (int i = 0; i < ListPlayer.Length; i++)
        {
            content[i] = new object[] { ListPlayer[i].GetComponent<Transform>().position , i};
            content[i] = new object[] { ListPlayer[i].GetComponent<PhotonView>().ViewID , i};
            
            Debug.Log("content["+i+"] = " + content[i]);
        }
        */
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
        
        /*      
         { AirDrop.RandomPosition(80f),
                    UnityEngine.Random.Range(0, 7) };
        */

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
            PunNetworkManager.singleton.SpawnPlayer();
        }
        else
        {
            Debug.Log("Ignoring scene load for " + SceneManagerHelper.ActiveSceneName);
        }

    }
    public void SpawnPlayer()
    {
        // we're in a room. spawn a character for the local player.
        // it gets synced by using PhotonNetwork.Instantiate
        PhotonNetwork.Instantiate(GamePlayerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        
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
                Keyboard kboard = Keyboard.current;
                if (kboard.rKey.wasPressedThisFrame)
                    if (kboard.rKey.keyCode == Key.R)
                        CallRaiseEvent();
                break;
        }
    }
    
    [PunRPC]
    void UpdatePosition(Vector3 pos, int viewID) {
        // Use the received position to update the game object's position
        GameObjectData data = new GameObjectData();
        data.viewID = viewID;
        data.position = pos;
        gameObjectList.Add(data);
        Debug.Log("View ID: " + data.viewID + " Position: " + data.position);
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
