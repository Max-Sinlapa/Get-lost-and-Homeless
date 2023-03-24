using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace IceDEV
{
    public class NextLevel : MonoBehaviour
    {
        public int nextSceneLoad;

        void Start()
        {
            nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "PlayerCat" && other.gameObject.name == "PlayerRat")
            {
                //Final Level
                if(SceneManager.GetActiveScene().name == "Level3")
                {
                    //LoadScene you win
                    SceneManager.LoadScene("YouWin", LoadSceneMode.Single);
                }
                else
                {
                    //MoveScene
                    SceneManager.LoadScene(nextSceneLoad);

                    //Setting Int for Index
                    if (nextSceneLoad > PlayerPrefs.GetInt("LevelAT"))
                    {
                        PlayerPrefs.SetInt("levelAt", nextSceneLoad);
                    }
                }

               
            }
                
        }
    }
}
