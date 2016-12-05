using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
    public abstract class InitiativeNumPadButton : MonoBehaviour
    {
        [SerializeField]
        protected Text playerInitiativeText;

        public abstract void OnButtonPress();
    }
}
