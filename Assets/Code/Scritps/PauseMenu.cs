using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool isPause = false;
    // Start is called before the first frame update
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && !isPause)
        {
            Pause();
            
        }
        else if (Input.GetKeyDown(KeyCode.O) && isPause)
        {
            Resume();
           
        }
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene("PauseGame", LoadSceneMode.Additive);
        isPause = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync("PauseGame");
        isPause = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
