﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionPanel : MonoBehaviour
{
    [Header("Selection Panel")]
    public Button _createRoomButton;
    public Button _joinRandomRoomButton;
    public Button _findMatchButton;
    public Button _RoomListButton;
    public Button _cancelButton;
    public string _cancelScene;


    private void Awake()
    {
        if (_createRoomButton != null)
            _createRoomButton.onClick.AddListener(OnCreateRoomButtonClicked);
        else
            Debug.LogError("Missing Create Room Button.");

        if (_joinRandomRoomButton != null)
            _joinRandomRoomButton.onClick.AddListener(OnJoinRandomRoomButtonClicked);
        else
            Debug.LogError("Missing join Random Room Button.");

        if (_findMatchButton != null)
            _findMatchButton.onClick.AddListener(OnFindMatchButtonClicked);
        else
            Debug.LogError("Missing Room List Room Button.");

        if (_RoomListButton != null)
            _RoomListButton.onClick.AddListener(OnRoomListButtonClicked);
        else
            Debug.LogError("Missing Room List Room Button.");
        
        if (_cancelButton != null)
            _cancelButton.onClick.AddListener(CancelButtonClicked);
        else
            Debug.LogError("Missing Cancel Button.");
    }
    

    void OnCreateRoomButtonClicked()
    {
        LobbyPanelManager.instance.SetActivePanel(LobbyPanelManager.panelName.CreateRoomPanel);
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        LobbyPanelManager.instance.SetActivePanel(LobbyPanelManager.panelName.JoinRandomRoomPanel);

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnFindMatchButtonClicked()
    {
        LobbyPanelManager.instance.AutoConnect = true;
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        LobbyPanelManager.instance.SetActivePanel(LobbyPanelManager.panelName.RoomLisPanel);
    }

    public void CancelButtonClicked()
    {
        PhotonNetwork.LocalPlayer.NickName = null;
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(_cancelScene, LoadSceneMode.Single);
    }

}
