using UnityEngine;
using UnityEngine.SceneManagement;

namespace Anonim
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadSceneByIndex(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }
}