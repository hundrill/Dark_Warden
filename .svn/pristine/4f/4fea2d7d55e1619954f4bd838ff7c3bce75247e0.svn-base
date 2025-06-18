namespace VideoPokerKit
{
    public class DealButton : GameButton
    {
        public override void PressAction()
        {
            // call the main deal function from MainGame
            // (it uses the static reference to acces the object instance)
            MainGame.the.DealCards();

            // play sound
            SoundsManager.the.dealButtonSound.Play();
        }

        //------------------------------------------

        public void Update()
        {
            // if we are in each of the DEALING states, disable the button
            // so that we can't press it
            if (MainGame.the.gameState == MainGame.STATE_DEALING ||
               MainGame.the.gameState == MainGame.STATE_DEALING2)
            {
                buttonCollider.enabled = false;
                highlightEnabled = false;
            }
            else
            {
                // otherwise the button is always enabled
                buttonCollider.enabled = true;
                highlightEnabled = true;
            }

            base.Update();
        }
    }
}