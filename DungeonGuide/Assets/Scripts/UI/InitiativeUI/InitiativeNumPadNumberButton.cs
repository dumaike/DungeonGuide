using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
    public class InitiativeNumPadNumberButton : InitiativeNumPadButton
    {
        private Text buttonNumber;

        private void Start()
        {
            buttonNumber = this.GetComponentInChildren<Text>();
        }

        public override void OnButtonPress()
        {

            //Clear out double clicks
            SceneManager.eventCtr.FireMenuItemClickedEvent();

            playerInitiativeText.text = playerInitiativeText.text + buttonNumber.text;
        }
    }
}
