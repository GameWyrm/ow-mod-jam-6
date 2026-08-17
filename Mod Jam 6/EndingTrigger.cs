using UnityEngine;

namespace Mod_Jam_6
{
    public class EndingTrigger : MonoBehaviour
    {
        private GameObject nhEndingTrigger;

        private void Start()
        {
            nhEndingTrigger = GameObject.Find("ENDING_TRIGGER");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (TimeLoop.IsTimeLoopEnabled())
                {
                    TimeLoop.SetSecondsRemaining(-200);
                    ModJam6.RevealFact("PH_LOG_OUTSIDE_5");
                    Locator.GetDeathManager().KillPlayer(DeathType.TimeLoop);
                }
                else if (EndingBarrier.Instance.hasWarpCore)
                {
                    nhEndingTrigger.transform.position = transform.position;
                }
            }
        }
    }
}
