using System.Collections.Generic;
using UnityEngine;

namespace Mod_Jam_6
{
    public class WispFusion : MonoBehaviour
    {
        [SerializeField]
        private float _angularThreshold = 1f;

        [SerializeField]
        private float _minDistance = 3f;
        [SerializeField]
        private float _maxDistance = 20f;

        [SerializeField]
        private GameObject[] _objectsToToggleOnEcho;
        [SerializeField]
        private GameObject[] _objectsToToggleOffEcho;
        [SerializeField]
        private GameObject[] _objectsToToggleOnRipple;
        [SerializeField]
        private GameObject[] _objectsToToggleOffRipple;

        [SerializeField]
        private GameObject _echoObject;
        [SerializeField]
        private GameObject _rippleObject;

        private bool _hasFusedEcho = false;
        private bool _hasFusedRipple = false;

        [SerializeField]
        private string _echoCondition;
        [SerializeField]
        private string _rippleCondition;
        [SerializeField]
        private string _completeCondition;

        private void Update()
        {
            if(_hasFusedEcho && _hasFusedRipple)
            {
                base.enabled = false;
                return;
            }

            if (CheckProjection())
            {
                if (_echoObject != null && _echoObject.activeSelf && CheckEcho())
                {
                    if(_objectsToToggleOffEcho != null)
                    {
                        foreach (var obj in _objectsToToggleOffEcho)
                        {
                            obj.SetActive(false);
                        }
                    }
                    if(_objectsToToggleOnEcho != null)
                    {
                        foreach (var obj in _objectsToToggleOnEcho)
                        {
                            obj.SetActive(true);
                        }
                    }
                    _hasFusedEcho = true;

                    if (_hasFusedRipple)
                    {
                        DialogueConditionManager.SharedInstance.SetConditionState(_rippleCondition, false);
                        DialogueConditionManager.SharedInstance.SetConditionState(_completeCondition, true);
                    }
                    else
                    {
                        DialogueConditionManager.SharedInstance.SetConditionState(_echoCondition, true);
                    }
                }

                if (_rippleObject != null && _rippleObject.activeSelf && CheckRipple())
                {
                    if (_objectsToToggleOffEcho != null)
                    {
                        foreach (var obj in _objectsToToggleOffRipple)
                        {
                            obj.SetActive(false);
                        }
                    }
                    if (_objectsToToggleOnEcho != null)
                    {
                        foreach (var obj in _objectsToToggleOnRipple)
                        {
                            obj.SetActive(true);
                        }
                    }
                    _hasFusedRipple = true;

                    if (_hasFusedEcho)
                    {
                        DialogueConditionManager.SharedInstance.SetConditionState(_echoCondition, false);
                        DialogueConditionManager.SharedInstance.SetConditionState(_completeCondition, true);
                    }
                    else
                    {
                        DialogueConditionManager.SharedInstance.SetConditionState(_rippleCondition, true);
                    }
                }
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

        private bool CheckEcho()
        {
            // Angle
            float angle = Vector3.Angle(_echoObject.transform.position - Locator.GetPlayerCamera().transform.position, Locator.GetPlayerCamera().transform.forward);
            if (angle > _angularThreshold) { return false; }

            return true;
        }
        private bool CheckRipple()
        {
            // Angle
            float angle = Vector3.Angle(_rippleObject.transform.position - Locator.GetPlayerCamera().transform.position, Locator.GetPlayerCamera().transform.forward);
            if (angle > _angularThreshold) { return false; }

            return true;
        }

        public void OnTriggerEnter(Collider hitCollider)
        {
            if (!(_hasFusedEcho && _hasFusedRipple) && hitCollider.CompareTag("PlayerDetector")) // Only enable when player close (and if not already completed)
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
