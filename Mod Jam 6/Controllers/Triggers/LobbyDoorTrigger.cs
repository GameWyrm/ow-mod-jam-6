
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
                ModJam6.Instance.ModHelper.Console.WriteLine("ENTERED LOBBY WITH PIC", OWML.Common.MessageType.Error);
                _isSolved = true;
                _bellhopMirrorTrigger.SetSolved();
            }
        }
    }
}
