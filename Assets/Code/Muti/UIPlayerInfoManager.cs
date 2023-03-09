using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfoManager : MonoBehaviour
{
    public Slider _HealthBar;
    
    
    public TextMeshProUGUI _TextNickName;
    //PunHealth _userHealth;
    

    PhotonView photonView;
    public void Awake() {
        //_userHealth = GetComponentInParent<PunHealth>();
        


        photonView = GetComponentInParent<PhotonView>();

        if (photonView.IsMine)
            SetLocalUI();
    }

    public void SetLocalUI()
    {
        //UI Control
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        //GetComponent<UIDirectionControl>().enabled = false;
    }

    public void SetNickName(string name)
    {
        if (_TextNickName != null)
            _TextNickName.text = name;
    }

    private void FixedUpdate()
    {
        //if (_HealthBar != null && _userHealth != null)
        {
            //int currentHealth = _userHealth.currentHealth;
            //_HealthBar.value = (float)currentHealth / (float)100;
        }

        //if (_HealthBar != null && _userHealth != null)
        {
            //int currentStermina = _userStermina.currentStermina;
            //int curentMana = _userStermina.currentMana;
            //_SterminaBar.value = (float)currentStermina / (float)100;
            //_ManaBar.value = (float)curentMana / (float)100;

        }
    }
}
