using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DungeonGuide
{
    public class ExperienceCalcDialog : MonoBehaviour
    {

        [SerializeField]
        private Text experienceText;

        [SerializeField]
        private Text numberOfPlayersText;

        [SerializeField]
        private Text experiencePerPlayerText;

        [SerializeField]
        private GameObject enterExperienceUi;

        [SerializeField]
        private GameObject experiencePerPlayerUi;

        private int numberOfPlayers;
        private int experienceTotal;

		private const int DEFAULT_NUM_PLAYERS = 5;

        public void ActivateDialog()
        {
            //Clear out double clicks
            SceneManager.eventCtr.FireMenuItemClickedEvent();

            this.gameObject.SetActive(true);
            this.enterExperienceUi.SetActive(true);
            this.experiencePerPlayerUi.SetActive(false);
            this.experienceText.text = "";
            this.numberOfPlayersText.text = DEFAULT_NUM_PLAYERS.ToString();
            int.TryParse(this.numberOfPlayersText.text, out numberOfPlayers);
        }

        public void IncreasePlayers()
        {
            ++numberOfPlayers;
            this.numberOfPlayersText.text = numberOfPlayers.ToString("N0");
        }

        public void DecreasePlayers()
        {
            if(numberOfPlayers<=0)
            {
                this.numberOfPlayersText.text = "0";
            }
            else if(numberOfPlayers>0)
            {
                --numberOfPlayers;
                this.numberOfPlayersText.text = numberOfPlayers.ToString("N0");
            }
        }

        public void Calculate()
        {
            int.TryParse(this.experienceText.text, out experienceTotal);
            int experiencePerPlayer = experienceTotal / numberOfPlayers;
            this.experiencePerPlayerText.text = experiencePerPlayer.ToString("0" + " XP");
            this.enterExperienceUi.SetActive(false);
            this.experiencePerPlayerUi.SetActive(true);
        }
    }
}
