using UnityEngine;

namespace Mod_Jam_6.Controllers
{
    public class SafeController : MonoBehaviour
    {
        public string beliefCondition;
        public GameObject warpCoreRoot;
        public bool isOpen;

        public void ToggleIsOpen()
        {
            isOpen = !isOpen;
            warpCoreRoot.SetActive(!isOpen);
            ModJam6.Log($"The belief condition is {DialogueConditionManager.SharedInstance.GetConditionState(beliefCondition)}");
            if (DialogueConditionManager.SharedInstance.GetConditionState(beliefCondition)) warpCoreRoot.SetActive(true);
        }

    }
}
