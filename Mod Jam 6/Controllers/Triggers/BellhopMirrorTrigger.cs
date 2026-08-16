
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
                base.enabled = false;
                _isSolving = false;
                return;
            }

            if (!_bellhopMirror.IsLockedByProbeSnapshot()) // Toggle back if photo no longer held
            {
                ToggleObjects(isEntry: false);
                base.enabled = false;
                _isSolving = false;
            }
        }

        private void ToggleObjects(bool isEntry)
        {
            if (_objectsToToggleOn != null)
            {
                foreach (var obj in _objectsToToggleOn)
                {
                    obj.SetActive(isEntry); // on entry, toggle ON
                }
            }
            if (_objectsToToggleOff != null)
            {
                foreach (var obj in _objectsToToggleOff)
                {
                    obj.SetActive(!isEntry); // on entry, toggle OFF
                }
            }
        }
    }
}
