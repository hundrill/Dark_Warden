using UnityEngine;

namespace VideoPokerKit
{
    public class PaytableButton : GameButton
    {
        public Animator paytableAnimator;

        public override void PressAction()
        {
            // play sound
            SoundsManager.the.buttonsSound.Play();
            // open info panel, start animation
            if (paytableAnimator)
                paytableAnimator.SetBool("show", !paytableAnimator.GetBool("show"));
        }
    }
}