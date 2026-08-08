
using UnityEngine;

namespace Mod_Jam_6.Controllers
{
    public class LobbyDoorTrigger : MonoBehaviour
    {
        [SerializeField]
        private BellhopMirror _bellhopMirror;

        [SerializeField]
        private GameObject[] _objectsToToggleOn;
        [SerializeField]
        private GameObject[] _objectsToToggleOff;

        private bool _hasBeenSolved = false;

        public void OnTriggerEnter(Collider hitCollider)
        {
            if(_hasBeenSolved) { return; }

            if (hitCollider.CompareTag("PlayerDetector") && _bellhopMirror.IsLockedByActiveCamera())
            {
                foreach (var obj in _objectsToToggleOff)
                {
                    obj.SetActive(false);
                }
                foreach (var obj in _objectsToToggleOn)
                {
                    obj.SetActive(true);
                }
                _hasBeenSolved = true;
            }
        }
    }
}
