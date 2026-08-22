using UnityEngine;
using System.Collections;

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
            ModJam6.RevealFact("PH_LOG_FURNACE_2");
            StartCoroutine(ResetEnabled());
        }

        // fix interactables requiring you to look away from them to use again
        private IEnumerator ResetEnabled()
        {
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(0.5f);
            GetComponent<Collider>().enabled = true;
        }
    }
}
