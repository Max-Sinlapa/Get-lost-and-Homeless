using UnityEngine;
using StarterAssets;
using Photon.Pun;
using UnityEngine.InputSystem;
using ExitGames.Client.Photon;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
public class PunUserNetControl : MonoBehaviourPunCallbacks , IPunInstantiateMagicCallback 
{
    [Header("My Option")]
    public MeshRenderer _teamReander;
    
    
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    public Transform CameraRoot;
    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        Debug.Log(info.photonView.Owner.ToString());
        Debug.Log(info.photonView.ViewID.ToString());
        // #Important
        // used in PunNetworkManager.cs
        // : we keep track of the localPlayer instance to prevent instanciation
        // when levels are synchronized
        if (photonView.IsMine) {
            LocalPlayerInstance = gameObject;
            GetComponentInChildren<MeshRenderer>().material.color = Color.blue;

            // Reference Camera on run-time
            PunNetworkManager.singleton._vCam.Follow = CameraRoot;
            
            
            // Reference Input on run-time
            PlayerInput _pInput = GetComponent<PlayerInput>(); 
            _pInput.actions = PunNetworkManager.singleton._inputActions;
            
        }
        else {
            GetComponent<ThirdPersonController>().enabled = false;
            OnPlayerPropertiesUpdate(photonView.Owner, photonView.Owner.CustomProperties);
        }
        
        //Setting Nickname
        GetComponentInChildren<UIPlayerInfoManager>().SetNickName(info.Sender.NickName);

        //Color
        OnPlayerPropertiesUpdate(info.Sender,info.Sender.CustomProperties);

        //Setting Mesh Team
        if (_teamReander != null)
            SettingPlayerTeam(info.Sender);
        
    }
    
    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps) 
    {
        base.OnPlayerPropertiesUpdate(target, changedProps);
        if (changedProps.ContainsKey(PunGameSetting.PLAYER_COLOR) && target.ActorNumber == photonView.ControllerActorNr) 
        {
            object colors;
            if (changedProps.TryGetValue(PunGameSetting.PLAYER_COLOR, out colors)) 
            {
                GetComponentInChildren<MeshRenderer>().material.color = PunGameSetting.GetColor((int)colors);
            }
            return;
        }
    }

    #region MyRegion

    //Lobby with Team
    private void SettingPlayerTeam(Player Sender)
    {
        Photon.Pun.UtilityScripts.PhotonTeam _currentTeam = Photon.Pun.UtilityScripts.PhotonTeamExtensions.GetPhotonTeam(Sender);
        if (_currentTeam != null)
        {
            //int colors = (int)TeamExtensions.GetTeam(Sender);
            int colors = (int)_currentTeam.Code;
            _teamReander.material.color = PunGameSetting.GetColor(colors);
        }
    }

    #endregion
}