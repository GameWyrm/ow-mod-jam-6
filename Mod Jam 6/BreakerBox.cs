using UnityEngine;

namespace Mod_Jam_6
{
    public class BreakerBox : MonoBehaviour
    {
        public static BreakerBox instance;

        public bool repaired;

        [SerializeField]
        private string promptText;

        private SingleInteractionVolume interact;

        private void Start()
        {
            instance = this;

            interact = this.GetRequiredComponent<SingleInteractionVolume>();
            interact.OnPressInteract += OnPressInteract;

            interact.ChangePrompt(ModJam6.Instance.NewHorizons.GetTranslationForUI(promptText));
        }

        private void OnPressInteract()
        {
            repaired = true;
            gameObject.SetActive(false);
        }
    }
}
