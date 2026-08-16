using System.Collections.Generic;
using UnityEngine;

namespace Mod_Jam_6
{
    public class BeliefProjection : MonoBehaviour
    {
        [SerializeField]
        private float _angularThreshold = 1f;

        [SerializeField]
        private float _minDistance = 3f;
        [SerializeField]
        private float _maxDistance = 20f;

        private bool _hasProjected = false;

        [SerializeField]
        private GameObject[] _objectsToToggleOn;
        [SerializeField]
        private GameObject[] _objectsToToggleOff;

        [SerializeField]
        private string _projectionCondition;

        private void Update()
        {
            if (!_hasProjected && CheckProjection()) // the !_hasProjected should be redundant (should not be enabled if _hasProjected is true) but keeping it in just in case
            {
                if(_objectsToToggleOff != null)
                {
                    foreach (var obj in _objectsToToggleOff)
                    {
                        obj.SetActive(false);
                    }
                }
                if (_objectsToToggleOn != null)
                {
                    foreach (var obj in _objectsToToggleOn)
                    {
                        obj.SetActive(true);
                    }
                }
                _hasProjected = true;
                if(_projectionCondition != null && _projectionCondition.Length > 0)
                {
                    DialogueConditionManager.SharedInstance.SetConditionState(_projectionCondition, true);
                }
                base.enabled = false;
            }
        }

        private bool CheckProjection()
        {
            // Has Signalscope and zoom
            if (!(Locator.GetToolModeSwapper().GetSignalScope().InZoomMode())) { return false; }

            // Distance
            float distance = Vector3.Distance(base.transform.position, Locator.GetPlayerCamera().transform.position);
            bool isCorrectDistance = _minDistance <= distance && distance <= _maxDistance;
            if (!isCorrectDistance) { return false; }

            // Angle
            float angle = Vector3.Angle(base.transform.position - Locator.GetPlayerCamera().transform.position, Locator.GetPlayerCamera().transform.forward);
            if (angle > _angularThreshold) { return false; }

            return true;
        }

        public void OnTriggerEnter(Collider hitCollider)
        {
            if (!_hasProjected && hitCollider.CompareTag("PlayerDetector")) // Only enable when player close (and if not already completed)
            {
                base.enabled = true;
            }
        }
        public void OnTriggerExit(Collider hitCollider)
        {
            base.enabled = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _minDistance);
            Gizmos.DrawWireSphere(transform.position, _maxDistance);
        }
    }
}
