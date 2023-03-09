using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Cinemachine;
using UnityEngine.InputSystem;
using System.Diagnostics.Tracing;
using ExitGames.Client.Photon;
using System;
using UnityEngine.InputSystem.Controls;

public class PunNetworkManager : ConnectAndJoinRandom, IOnEventCallback
{
    public static PunNetworkManager singleton;
    //public bool isUseMainCamera;
    public CinemachineVirtualCamera _vCam;
    public InputActionAsset _inputActions;

    [Header("Spawn Info")]
    [Tooltip("The prefab to use for representing the player")]
    public GameObject GamePlayerPrefab;
    public GameObject _SpawnPlayer;

    public enum gamestate
    {
        None = 0,
        GameStart = 1,
        GamePlay = 2,
        GameOver = 3
    }

    public gamestate _currentGamestate = gamestate.GameStart;
    public gamestate CurrentGamestate
    {
        get { return _currentGamestate; }
        set {
            _currentGamestate = value;

            if (PhotonNetwork.CurrentRoom == null)
                return;

            Hashtable props = new Hashtable
            {
                { PunGameSetting.GAMESTATE, _currentGamestate.ToString() }
            };
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

    /// <summary>
    /// Prefab for Resie Event
    /// </summary>
    public GameObject RocketPrefab;

    // Raise Event
    // Custom Event 10: Used as "RandomCallAirDropEvent" event
    private readonly byte RandomCallAirDropEvent = 10;

    public override void OnEnable()
    {
        base.OnEnable();
        // Raise Event
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        // Raise Event
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    // RaiseEvent with Local GameObject.
    private void CallRaiseEvent()
    {
        // Array contains the target position and the IDs of the selected units
        //object[] content = new object[] { AirDrop.RandomPosition(80f), UnityEngine.Random.Range(0, 7) };
        // You would have to set the Receivers to All in order to receive this event on the local client as well
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true, Encrypt = true };

        //PhotonNetwork.RaiseEvent(RandomCallAirDropEvent, content, raiseEventOptions, sendOptions);
        Debug.Log("Call Raise Event.");
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log(photonEvent.ToStringFull());

        byte eventCode = photonEvent.Code;

        if (eventCode == RandomCallAirDropEvent)
        {

            Debug.Log("Call Resise Event is : " + eventCode.ToString());
            object[] data = (object[])photonEvent.CustomData;

            Vector3 position = (Vector3)data[0];
            int color = (int)data[1];
            Debug.Log("Position : " + position);
            Debug.Log("Color : " + color);
            // Instance Local Object
            GameObject localRocket = Instantiate(RocketPrefab);
            Color currentColor = PunGameSetting.GetColor(color);
            localRocket.transform.position = position;
            //localRocket.GetComponent<RocketBoom>().Damage *= color;
            localRocket.GetComponent<MeshRenderer>().material.color = currentColor;
        }
    }


    private void Awake()
    {
        singleton = this;

        //Add Reference Method to Delegate Method
        OnGameStart += GameStartSetting;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Debug.Log("New Player. " + newPlayer.ToString());
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (PunUserNetControl.LocalPlayerInstance == null)
        {
            Debug.Log("We are Instantiating LocalPlayer from " + SceneManagerHelper.ActiveSceneName);
            //PunNetworkManager.singleton.SpawnPlayer();
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(GamePlayerPrefab.name,
                _SpawnPlayer.transform.position, Quaternion.identity, 0);

            //isGameStart = true;
            CurrentGamestate = gamestate.GameStart;
            PunNetworkManager.singleton.SpawnPlayer();
        }
        else
        {
            Debug.Log("Ignoring scene load for " + SceneManagerHelper.ActiveSceneName);
        }
    }

    private void GameStartSetting()
    {
        CurrentGamestate = gamestate.GamePlay;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        switch(_currentGamestate)
        {
            case gamestate.GameStart:
                OnGameStart();
                break;

            case gamestate.GamePlay:
                //Game Loop Logic
                Keyboard kboard = Keyboard.current;

                if (kboard.rKey.wasPressedThisFrame)
                    if (kboard.rKey.keyCode == Key.R)
                        CallRaiseEvent();

                break;
        }
    }

    public void gameStateUpdate(Hashtable propertiesThatChanged)
    {
        object gameStateFromProps;

        if (propertiesThatChanged.TryGetValue(PunGameSetting.GAMESTATE, out gameStateFromProps))
        {
            Debug.Log("GetStartTime Prop is : " + gameStateFromProps);
            _currentGamestate = (gamestate)Enum.Parse(typeof(gamestate), (string)gameStateFromProps);
        }

        if(_currentGamestate == gamestate.GameOver)
            OnGameOver();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        gameStateUpdate(propertiesThatChanged);
    }

}
