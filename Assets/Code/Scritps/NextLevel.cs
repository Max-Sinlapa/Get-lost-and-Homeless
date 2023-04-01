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
        private bool target1Detected = false;
        private bool target2Detected = false;
        void Start()
        {
            nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "PlayerCat")
            {

                target1Detected = true;
            }
            else if (other.gameObject.name == "PlayerRat")
            {

                target2Detected = true;
            }

            if(target1Detected && target2Detected)
            {
                //Final Level
                if(SceneManager.GetActiveScene().name == "G_GameplaySingle-3")
                {
                    //LoadScene you win
                    SceneManager.LoadScene("_Completed", LoadSceneMode.Single);
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
}
