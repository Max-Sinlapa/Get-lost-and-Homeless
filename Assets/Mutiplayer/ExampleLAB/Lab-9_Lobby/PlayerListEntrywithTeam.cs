using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
public class PlayerListEntrywithTeam : PlayerListEntry {
    
    public Dropdown PlayerTeamDropdown;
    private bool isTeamMode;
    // Start is called before the first frame update
    public override void Start() {
        base.Start();
        
       // Debug.Log("ownerId = " +ownerId);
       // Debug.Log("PhotonNetwork.LocalPlayer.ActorNumber = " +PhotonNetwork.LocalPlayer.ActorNumber);
        if (isTeamMode) {
            
            //Debug.Log("isTeamMode = " +isTeamMode);
            PlayerTeamDropdown.gameObject.SetActive(true);
            PhotonTeam[] _listTeam = PhotonTeamsManager.Instance.
                GetAvailableTeams();
            foreach (PhotonTeam team in _listTeam) {
                PlayerTeamDropdown.options.Add(new Dropdown.OptionData(team.Name));
            }
            //PhotonTeamExtensions.JoinTeam(PhotonNetwork.LocalPlayer, (byte)(PlayerTeamDropdown.value + 1));
            PhotonTeamExtensions.JoinTeam(PhotonNetwork.LocalPlayer, (byte)(PhotonNetwork.CurrentRoom.PlayerCount));

        }
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId) {
            PlayerTeamDropdown.interactable = false;
        } else {
            PlayerTeamDropdown.onValueChanged.AddListener((int currentValue) =>
            {
                int plusValue = currentValue + 1;
                print("Change Value : " + plusValue);
                PhotonTeamExtensions.SwitchTeam(PhotonNetwork.LocalPlayer, (byte)plusValue);
            });
        }
    }
    public void Initialize(int playerId, string playerName, bool isTeamMode) {
        base.Initialize(playerId, playerName);
        this.isTeamMode = isTeamMode;
    }
    
    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps) {
        base.OnPlayerPropertiesUpdate(target, changedProps);
        
        //Debug.Log("OnPlayerPropertiesUpdate");
        PhotonTeam _currentTeam = PhotonTeamExtensions.GetPhotonTeam(target);
        
        if(_currentTeam != null ) 
        {
            if (this.ownerId == target.ActorNumber)
            {
               int currentTeam = (int)_currentTeam.Code;
                           Debug.Log("Player" + PhotonNetwork.LocalPlayer);
                           Debug.Log("Player=target" + target);
                           print("Current Team: " + currentTeam);
                           PlayerTeamDropdown.value = currentTeam - 1;
                           return; 
            }
        }
        else
        {
            Debug.Log("_currentTeam = null");
        }
    }
}