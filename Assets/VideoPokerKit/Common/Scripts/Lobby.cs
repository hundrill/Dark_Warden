using UnityEngine;
using UnityEngine.SceneManagement;

namespace VideoPokerKit
{
    public class Lobby : MonoBehaviour
    {
        // launch each template from its own Unity scene

        //-------------------------------------------

        public void LaunchClassicTemplate()
        {
            SceneManager.LoadScene("t1classic");
        }

        //-------------------------------------------

        public void LaunchStyleTemplate()
        {
            SceneManager.LoadScene("t2style");
        }

        //-------------------------------------------

        public void LaunchArcadeTemplate()
        {
            SceneManager.LoadScene("t3Arcade");
        }
    }
}