
using UnityEngine;

namespace Mod_Jam_6
{
    public class LobbyDoorTrigger : MonoBehaviour
    {
        [SerializeField]
        private BellhopMirrorTrigger _bellhopMirrorTrigger;

        private bool _isSolved = false;

        public void OnTriggerEnter(Collider hitCollider)
        {
            if (_isSolved) { return; }

            if (hitCollider.CompareTag("PlayerDetector"))
            {
                Locator.GetShipLogManager().RevealFact("PH_LOG_HOTEL_CURIOSITY_RUMOR_1"); // Unrelated but triggerbox also fits
                Locator.GetShipLogManager().RevealFact("PH_LOG_OUTSIDE_RUMOR_1"); // Unrelated but triggerbox also fits

                if (_bellhopMirrorTrigger.IsSolving())
                {
                    _isSolved = true;
                    _bellhopMirrorTrigger.SetSolved();
                    Locator.GetShipLogManager().RevealFact("PH_LOG_OUTSIDE_1");
                }
            }

        }
    }
}
