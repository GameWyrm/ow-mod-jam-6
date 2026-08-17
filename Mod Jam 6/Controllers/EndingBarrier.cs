using UnityEngine;

namespace Mod_Jam_6
{
    public class EndingBarrier : MonoBehaviour
    {
        public static EndingBarrier Instance;

        public GameObject barrier;
        public bool hasWarpCore;

        private WarpCoreItem warpCore;

        private void Start()
        {
            Instance = this;
            warpCore = transform.Find("Sector_Hotel/Sector/Sector_ThirdFloor/Safe/WarpCoreAnchor/AdvancedWarpCore").GetComponent<WarpCoreItem>();
            warpCore.onPickedUp.AddListener(PickUp);
        }

        private void PickUp(OWItem item)
        {
            barrier.SetActive(false);
            hasWarpCore = true;
            if (!TimeLoop.IsTimeLoopEnabled())
            {
                if (!DialogueConditionManager.SharedInstance.ConditionExists("PH_SHOW_NEWCOMER")) DialogueConditionManager.SharedInstance.AddCondition("PH_SHOW_NEWCOMER");
                DialogueConditionManager.SharedInstance.SetConditionState("PH_SHOW_NEWCOMER", true);
            }
        }
    }
}
