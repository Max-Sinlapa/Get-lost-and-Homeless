using System;
using UnityEngine;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun;
using UnityEngine.UI;

public class PunLaunchExt : Launcher
{
    public float TimeCountDownBeforeEnterRoom;
    private bool contudown = false;
    
    private void Start()
    {
        contudown = false;
        
        Debug.Log("We are Try To Connect Photon Server." +
                SceneManagerHelper.ActiveSceneName);
        TryConnect();
        //Connect();
        
    }

    public void TryConnect()
    {
        // we want to make sure the log is clear everytime we connect, we might have several failed attempted if connection failed.
        feedbackText.text = "";

        // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
        isConnecting = true;

        // hide the Play button for visual consistency
        controlPanel.SetActive(false);

        // start the loader animation for visual effect.
        if (loaderAnime != null)
        {
            loaderAnime.StartLoaderAnimation();
        }

        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (!PhotonNetwork.IsConnected){

            LogFeedback("Connecting...");

            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        // we don't want to do anything if we are not attempting to join a room. 
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        if (isConnecting)
        {
            LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");

            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            //PhotonNetwork.JoinRandomRoom();
            controlPanel.SetActive(true);
            loaderAnime.StopLoaderAnimation();
        }
    }

    public override void OnJoinedRoom()
    {
        LogFeedback("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {

            // #Critical
            // Load the Room Level.
            contudown = true;

        }
    }

    private void Update()
    {
        if (contudown == true)
        {
            TimeCountDownBeforeEnterRoom -= Time.deltaTime;
            if (TimeCountDownBeforeEnterRoom <= 0)
            {
                Debug.Log("We load the 'Room for 1' ");
                PhotonNetwork.LoadLevel(1);
                contudown = false;
            }
        }
       
    }
}
