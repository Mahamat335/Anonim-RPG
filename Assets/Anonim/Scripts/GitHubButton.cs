using UnityEngine;

namespace Anonim
{
    public class GitHubButton : MonoBehaviour
    {
        private string _gitHubUrl = "https://github.com/Mahamat335/Anonim-RPG";

        public void OpenGitHubRepo()
        {
            Application.OpenURL(_gitHubUrl);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}