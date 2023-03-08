using System.Collections;
using System.Collections.Generic;
using Max_DEV;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class m_SceneManager : MonoBehaviour
{
        public TextMeshProUGUI ScoreText;
        public string previusScene;

        private void Start()
        {
            previusScene = SceneManager.GetActiveScene().name;
        }

        private void Update()
        {
            /*
            if(previusScene == "Stage_1")
                ScoreText.text = "Score : " + Mathf.Floor(GameManager_1.Master_Score);
            if(previusScene == "Stage_2")
                ScoreText.text = "Score : " + Mathf.Floor(GameManager_2.Master_Score);
            else
                return;
            */
        }

        public static void LoadSingleScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        public void UnLoadSingleScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        
        //-----------------------------------------------------------------------------------
        
        public static void Load_PopUP_Scene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        public static void UNLoad_PopUP_Scene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        
        //-------------------------------------------------------------------------------------
        
        public static void Load_NextGamePlay_Scene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        public static void UNLoad_CurrentGamePlay_Scene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        
        //-------------------------------------------------------------------------------------

        /*
        public static void Load_LoseScene(string sceneName, int Score)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        }
        public void UnLoad_LoseScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            SceneManager.LoadScene(previusScene, LoadSceneMode.Single);
            
        }
        */
        
        //--------------------------------------------------------------------------------------
        
        public void ExitGame()
        {
            Application.Quit();
        }
        
        //---------------------------------------------------------------------------------------
        
        #region Scene Load and Unload Events Handler
        private void OnEnable()
        {
            SceneManager.sceneUnloaded += SceneUnloadEventHandler;
            SceneManager.sceneLoaded += SceneLoadedEventHandler;
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= SceneUnloadEventHandler;
            SceneManager.sceneLoaded -= SceneLoadedEventHandler;
        }
        
        private void SceneUnloadEventHandler(Scene scene)
        {
        }

        private void SceneLoadedEventHandler(Scene scene, LoadSceneMode mode)
        {
        }
        #endregion
}
