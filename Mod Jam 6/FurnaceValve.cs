using UnityEngine;

namespace Mod_Jam_6
{
    public class FurnaceValve : MonoBehaviour
    {
        public bool clockwise;

        private SingleInteractionVolume interact;

        private void Start()
        {
            interact = this.GetRequiredComponent<SingleInteractionVolume>();
            interact.OnPressInteract += OnPressInteract;
            interact.ChangePrompt(ModJam6.NewHorizons.GetTranslationForUI(clockwise ? "$PH_VALVE_CLOCKWISE" : "$PH_VALVE_COUNTERCLOCKWISE"));
        }

        private void OnPressInteract()
        {
            FurnaceController.Instance.TurnValve(clockwise);
        }
    }
}
