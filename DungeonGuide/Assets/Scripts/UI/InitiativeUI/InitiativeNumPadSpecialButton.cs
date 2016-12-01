using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
    public class InitiativeNumPadSpecialButton : InitiativeNumPadButton
    {
        private enum ButtonType
        {
            BACKSPACE,
            NEGATIVE_TOGGLE
        }

        [SerializeField]
        private ButtonType buttonType;

        public override void OnButtonPress()
        {

            //Clear out double clicks
            SceneManager.eventCtr.FireMenuItemClickedEvent();

            if (buttonType == ButtonType.BACKSPACE && playerInitiativeText.text.Length !=0)
            {

                playerInitiativeText.text = playerInitiativeText.text.Substring(0, playerInitiativeText.text.Length - 1);

            }

            else if (buttonType == ButtonType.NEGATIVE_TOGGLE && playerInitiativeText.text.Length !=0)
            {
                if (playerInitiativeText.text.Substring(0, 1) == "-")
                {
                    playerInitiativeText.text = playerInitiativeText.text.Substring(1);
                }
                else
                {
                    playerInitiativeText.text = "-" + playerInitiativeText.text;
                }
            }

        }
    }
}
