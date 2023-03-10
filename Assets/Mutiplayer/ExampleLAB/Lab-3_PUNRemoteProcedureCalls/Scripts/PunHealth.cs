using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PunHealth : MonoBehaviourPun, IPunObservable
{
    public const int maxHealth = 100;
    public int currentHealth = maxHealth;

    public void OnGUI() {
        if (photonView.IsMine)
            GUI.Label(new Rect(0, 0, 300, 50), 
                "Player Health : " + currentHealth);
    }

    public void TakeDamage(int amount, int OwnerNetID) {
        print("TakeDamage Call");
        if (photonView != null)
            photonView.RPC("PunRPCApplyHealth", 
                RpcTarget.MasterClient, 
                amount * -1, 
                OwnerNetID);
        else print("photonView is NULL.");
    }

    public void HealingPoint(int amount, int OwnerNetID) {
        if (photonView != null)
            photonView.RPC("PunRPCApplyHealth", 
                photonView.Owner, 
                amount, 
                OwnerNetID);
        else print("photonView is NULL.");
    }

    [PunRPC]
    public void PunRPCApplyHealth(int amount, int OwnerNetID) {
        print("PunRPCApplyHealth Call");

        Debug.Log("Update @" + PhotonNetwork.LocalPlayer.ActorNumber + 
                  " Apply Health : " + amount + " form : " + OwnerNetID);
        currentHealth += amount;
        if (currentHealth <= 0) {
            Debug.Log("NetID : " + OwnerNetID.ToString() + 
                      " Killed " + photonView.ViewID);
            photonView.RPC("PunResetPlayer", RpcTarget.All);
        }else {
            photonView.RPC("PunUpdateHealth", RpcTarget.Others,currentHealth);
        }
    }

    [PunRPC]
    public void PunUpdateHealth(int ownerHealth) {
        print("PunUpdateHealth Call");
        currentHealth = ownerHealth;
    }

    [PunRPC]
    public void PunResetPlayer() {
        Debug.Log("Reset Position..");
        Vector3 Direction = RandomPosition(20) - this.transform.position;
        this.GetComponent<CharacterController>().Move(Direction);
        currentHealth = maxHealth;
    }

    public Vector3 RandomPosition(float yOffset) {
        var spawnPosition = new Vector3(
            Random.Range(-30.0f, 30.0f),
            yOffset,
            Random.Range(-30.0f, 30.0f));
        return spawnPosition;
    }
    
    public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(currentHealth);
        }
        else {
            currentHealth = (int)stream.ReceiveNext();
        }
    }
}

