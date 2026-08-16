using UnityEngine;

namespace Mod_Jam_6
{
    public class EndLoop : MonoBehaviour
    {
        private SingleInteractionVolume interact;
        [SerializeField]
        private string interactText;

        private void Start ()
        {
            interact = this.GetRequiredComponent<SingleInteractionVolume>();
            interact.OnPressInteract += OnPressInteract;

            interact.ChangePrompt(ModJam6.NewHorizons.GetTranslationForUI(interactText));
        }

        private void OnPressInteract()
        {
            TimeLoop.SetTimeLoopEnabled(false);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
