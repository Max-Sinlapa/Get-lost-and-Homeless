using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalNextLevel : MonoBehaviour
{
    // Start is called before the first frame update

    public string SceneName;
    private bool target1Detected = false;
    private bool target2Detected = false;

    public void Awake()
    {
        target1Detected = false;
        target2Detected = false;
}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerCat")
        {
            
            target1Detected = true;
        }
        else if (other.gameObject.name == "PlayerRat")
        {
           
            target2Detected = true;
        }

        if (target1Detected && target2Detected)
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerCat")
        {
           
            target1Detected = false;
        }
        else if (other.gameObject.name == "PlayerRat")
        {
            
            target2Detected = false;
        }
    }
}

