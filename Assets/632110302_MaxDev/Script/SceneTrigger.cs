using System.Collections;
using System.Collections.Generic;
using Max_DEV;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;


public class SceneTrigger : MonoBehaviourPun
{

    public bool _CheckCat;
    public bool _CheckRat;
    public string _scene;

    public UnityEvent cutSceneTrigger;

    public bool multiplayer;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("object = " + other.gameObject);

        
        ObjectType_Identities OtherType = other.GetComponent<ObjectType_Identities>();
        if (OtherType != null)
        {
            switch (OtherType.Type)
            {
                case ObjectType.Cat:
                    if (_CheckCat)
                    {
                        if (multiplayer)
                        {
                            this.photonView.RPC("InWokeSceneTrigger", RpcTarget.All, _scene);
                        }
                        else
                        {
                            Debug.Log("SceneTrigger IN-WORK");
                            cutSceneTrigger.Invoke();
                        }
                        
                    }
                    break;
                case ObjectType.Mouse:
                    if (_CheckRat)
                    {
                        if (multiplayer)
                        {
                            this.photonView.RPC("InWokeSceneTrigger", RpcTarget.All, _scene);
                        }
                        else
                        {
                            Debug.Log("SceneTrigger IN-WORK");
                            cutSceneTrigger.Invoke();
                        }
                    }
                    break;
            }
        }
    }
    
    [PunRPC]
    public void InWokeSceneTrigger(string scene)
    {
        PhotonNetwork.LoadLevel(scene);
        //m_SceneManager.Multiplayer_ChangeScene(scene);
        Debug.Log("InWokeSceneTrigger WAKE");
    }
}
