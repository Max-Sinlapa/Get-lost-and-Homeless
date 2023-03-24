﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Max_DEV.Manager;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;
using UnityEngine.UI;

public class InsideRoomPanel : MonoBehaviourPunCallbacks , IOnEventCallback
{
    [Header("Inside Room Panel")]
    public Button LeaveGameButton;
    public Button StartGameButton;
    public GameObject PlayerListEntryPrefab;

    private Dictionary<int, GameObject> playerListEntries;

    // Raise Event
    // Custom Event 10: Used as "SetPlayerHealthInManager" event
    public readonly byte SetPlayerHealthInManager = 10;
    
    public override void OnEnable()
    {
        base.OnEnable();
        AwakeJoinedRoom();
        PhotonNetwork.AddCallbackTarget(this);
        ///PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    
    public override void OnDisable()
    {
        base.OnEnable();
        PhotonNetwork.RemoveCallbackTarget(this);
        ///PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void AwakeJoinedRoom()
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(this.transform);
            entry.transform.localScale = Vector3.one;
            SetupPlayerList(p, entry);

            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(PunGameSetting.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
            }

            playerListEntries.Add(p.ActorNumber, entry);
        }

        StartGameButton.onClick.RemoveAllListeners();
        StartGameButton.onClick.AddListener(OnStartGameButtonClicked);
        StartGameButton.gameObject.SetActive(CheckPlayersReady());

        LeaveGameButton.onClick.RemoveAllListeners();
        LeaveGameButton.onClick.AddListener(OnLeaveGameButtonClicked);

        Hashtable props = new Hashtable
            {
                {PunGameSetting.PLAYER_LOADED_LEVEL, false}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    protected virtual void SetupPlayerList(Player p, GameObject instanceEntry)
    {
        instanceEntry.GetComponent<PlayerListEntry>().Initialize(p.ActorNumber, p.NickName);
    }

    #region PUN Callback

    public override void OnLeftRoom()
    {
        LobbyPanelManager.instance.SetActivePanel(LobbyPanelManager.panelName.SelectionPanel);


        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        playerListEntries.Clear();
        playerListEntries = null;

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        GameObject entry = Instantiate(PlayerListEntryPrefab);
        entry.transform.SetParent(this.transform);
        entry.transform.localScale = Vector3.one;

        SetupPlayerEnteredRoom(newPlayer, entry);

        playerListEntries.Add(newPlayer.ActorNumber, entry);

        StartGameButton.gameObject.SetActive(CheckPlayersReady());

    }

    public virtual void SetupPlayerEnteredRoom(Player newPlayer, GameObject instanceEntry)
    {

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);

        StartGameButton.gameObject.SetActive(CheckPlayersReady());

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue(PunGameSetting.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
            }
        }

        StartGameButton.gameObject.SetActive(CheckPlayersReady());

    }

    #endregion

    #region UI CALLBACKS

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnStartGameButtonClicked()
    {
        
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        Debug.Log("InsideRoomPanel-OnStartGameButtonClicked CALL");
        //CallSetPlayerHealthEvent();
        m_GameManager.Set_Start_PlayerHealth(LobbyPanelManager.instance._playerHealth);
        m_GameManager.SetPlayerHealth(LobbyPanelManager.instance._playerHealth);
        
        Debug.Log("panel HP = " + LobbyPanelManager.instance._playerHealth);
        PhotonNetwork.LoadLevel(LobbyPanelManager.instance.GamePlayScene);
    }

    // Call form PlayerListEntry.cs
    public void LocalPlayerPropertiesUpdated()
    {
        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    #endregion

    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(PunGameSetting.PLAYER_READY, out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private void CallSetPlayerHealthEvent()
    {
        Debug.Log("CallSetPlayerHealthEvent");
        
        object[] data = new object[]
        {
            LobbyPanelManager.instance._playerHealth
        };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others ,
            CachingOption = EventCaching.AddToRoomCache
        };
        SendOptions sendOptions = new SendOptions
        {
            Reliability = true
        };

        Debug.Log("HealthForm CallSetPlayerHealthEvent = " + (int)data[0]);
        PhotonNetwork.RaiseEvent(SetPlayerHealthInManager, data, raiseEventOptions, sendOptions);
    }

    public void OnEvent(EventData photonEvent)
    {
        //throw new System.NotImplementedException();
    }
}
