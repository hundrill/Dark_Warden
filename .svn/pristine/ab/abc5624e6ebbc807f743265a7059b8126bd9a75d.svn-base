using UnityEngine.SceneManagement;

namespace VideoPokerKit
{
    public class MenuButton : GameButton
    {
        public override void PressAction()
        {
            // play sound
            SoundsManager.the.buttonsSound.Play();

            // go to lobby
            SceneManager.LoadScene("lobby");
        }
    }
}