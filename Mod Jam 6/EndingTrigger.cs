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
                    Locator.GetDeathManager().KillPlayer(DeathType.TimeLoop);
                }
                else
                {
                    nhEndingTrigger.transform.position = transform.position;
                }
            }
        }
    }
}
