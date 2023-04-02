using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool isPause = false;
    public string PauseSceneName;
    // Start is called before the first frame update
    public void Awake()
    {
        isPause = false;
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isPause)
        {
            Pause();
            
        }
        
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(PauseSceneName, LoadSceneMode.Additive);
        isPause = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync("_PauseGame");
        isPause = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
