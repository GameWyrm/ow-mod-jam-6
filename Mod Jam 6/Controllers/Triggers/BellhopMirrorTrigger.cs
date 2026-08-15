
using UnityEngine;

namespace Mod_Jam_6
{
    public class BellhopMirrorTrigger : MonoBehaviour
    {
        [SerializeField]
        private BellhopMirror _bellhopMirror;

        [SerializeField]
        private GameObject[] _objectsToToggleOn;
        [SerializeField]
        private GameObject[] _objectsToToggleOff;

        private bool _isSolving = false;
        private bool _isSolved = false;

        public void OnTriggerEnter(Collider hitCollider)
        {
            if (hitCollider.CompareTag("PlayerDetector"))
            {
                ModJam6.Instance.ModHelper.Console.WriteLine("ENTERED BELLHOP ROOM", OWML.Common.MessageType.Error);
                ToggleObjects(isEntry: true);
            }
        }
        public void OnTriggerExit(Collider hitCollider)
        {
            if (hitCollider.CompareTag("PlayerDetector"))
            {
                base.enabled = true;
                _isSolving = true;
            }
        }

        public bool IsSolving() => _isSolving;
        public void SetSolved()
        {
            _isSolved = true;
            _isSolving = false;
        }

        private void Update()
        {
            if (_isSolved)
            {
                ModJam6.Instance.ModHelper.Console.WriteLine("MIRROR IS SOLVED", OWML.Common.MessageType.Error);
                base.enabled = false;
                _isSolving = false;
                return;
            }

            if (!_bellhopMirror.IsLockedByProbeSnapshot()) // Toggle back if photo no longer held
            {
                ModJam6.Instance.ModHelper.Console.WriteLine("STOPPED LOOKING AT PIC", OWML.Common.MessageType.Error);
                ToggleObjects(isEntry: false);
                base.enabled = false;
                _isSolving = false;
            }
        }

        private void ToggleObjects(bool isEntry)
        {
            if (_objectsToToggleOn != null)
            {
                ModJam6.Instance.ModHelper.Console.WriteLine($"TURNING {_objectsToToggleOn.Length} OBJECTS ON {isEntry}", OWML.Common.MessageType.Error);
                foreach (var obj in _objectsToToggleOn)
                {
                    obj.SetActive(isEntry); // on entry, toggle ON
                }
            }
            if (_objectsToToggleOff != null)
            {
                ModJam6.Instance.ModHelper.Console.WriteLine($"TURNING {_objectsToToggleOff.Length} OBJECTS OFF {isEntry}", OWML.Common.MessageType.Error);
                foreach (var obj in _objectsToToggleOff)
                {
                    obj.SetActive(!isEntry); // on entry, toggle OFF
                }
            }
        }
    }
}
