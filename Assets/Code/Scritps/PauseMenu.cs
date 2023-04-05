using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool isPause = false;
    public string PauseSceneName;

   
    // Start is called before the first frame update
    public void Awake()
    {
        //isPause = false;
    }
    public void Update()
    {
        //Debug.Log("isPause = " + isPause);
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPause)
            {
                Pause();
            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void Pause()
    {
        //SceneManager.GetActiveScene();
        //Debug.Log("inwoke Pause" + isPause);
        Time.timeScale = 0f;
        SceneManager.LoadScene(PauseSceneName, LoadSceneMode.Additive);
        isPause = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //Debug.Log("inwoke Pause-2" + isPause);
    }

    public void Resume()
    {
        //Debug.Log("inwoke Resume" + isPause);
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync("_PauseGame");
        isPause = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //Debug.Log("inwoke Resume-2" + isPause);

    }
}
