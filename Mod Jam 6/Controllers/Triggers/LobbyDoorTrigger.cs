
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

            if (hitCollider.CompareTag("PlayerDetector") && _bellhopMirrorTrigger.IsSolving())
            {
                _isSolved = true;
                _bellhopMirrorTrigger.SetSolved();
            }
        }
    }
}
