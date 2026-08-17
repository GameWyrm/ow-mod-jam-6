using UnityEngine;

namespace Mod_Jam_6
{
    public class EndLoop : MonoBehaviour
    {
        public static EndLoop instance;

        private SingleInteractionVolume interact;
        [SerializeField]
        private string interactText;
        [SerializeField]
        private string secondaryInteractText;

        private void Start ()
        {
            instance = this;

            interact = this.GetRequiredComponent<SingleInteractionVolume>();
            interact.OnPressInteract += OnPressInteract;

            interact.ChangePrompt(ModJam6.NewHorizons.GetTranslationForUI(interactText));
        }

        private void OnPressInteract()
        {
            if (TimeLoop._timeLoopEnabled)
            {
                TimeLoop.SetTimeLoopEnabled(false);
                GetComponentInParent<MeshRenderer>().enabled = false;
                if (EndingBarrier.Instance.hasWarpCore)
                {
                    if (!DialogueConditionManager.SharedInstance.ConditionExists("PH_SHOW_NEWCOMER")) DialogueConditionManager.SharedInstance.AddCondition("PH_SHOW_NEWCOMER");
                    DialogueConditionManager.SharedInstance.SetConditionState("PH_SHOW_NEWCOMER", true);
                }
                interact.ChangePrompt(ModJam6.NewHorizons.GetTranslationForUI(secondaryInteractText));
            }
            else
            {
                TimeLoop.SetTimeLoopEnabled(true);
                GetComponentInParent<MeshRenderer>().enabled = true;
                interact.ChangePrompt(ModJam6.NewHorizons.GetTranslationForUI(interactText));
            }
            ModJam6.RevealFact("PH_LOG_BASEMENT_2");
        }
    }
}
