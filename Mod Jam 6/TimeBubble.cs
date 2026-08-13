using UnityEngine;

namespace Mod_Jam_6
{
    public class TimeBubble : MonoBehaviour
    {
        public static TimeBubble instance;

        private GameObject[] _objectsToToggleOn;
        private GameObject[] _objectsToToggleOff;
        private GameObject[] _objectsToToggleBackOn;
        private GameObject[] _objectsToToggleBackOff;

        private float _size;
        public float size => _size;

        private bool _isShrunk;
        private bool _isExpanded;
        private bool _isExpanding;
        private bool _isShrinking;
        private bool _wishToExpand;

        private Vector3 _nextPosition;
        private float _nextExpandedSize;
        private float _nextExpansionDuration;
        private float _nextCollapseDuration;
        private float _currentExpandedSize;
        private float _currentExpansionDuration;
        private float _currentCollapseDuration;

        private float _lerpTimeStart;
        private float _lerpTimeCurrent;

        private TimeBubbleActivator _currentActivator;


        private void Start()
        {
            instance = this;

            _size = 0f;
            _isShrunk = true;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _size);
        }

        public void TryActivate(Vector3 newPosition, float newExpandedSize, float newExpansionDuration, float newCollapseDuration, GameObject[] objectsToToggleOn, GameObject[] objectsToToggleOff, TimeBubbleActivator activator)
        {
            if (_currentActivator != null) _currentActivator._isActive = false;
            _currentActivator = activator;
            // Case 1: Activating where the bubble already is. This is simple, we just tell it to expand.
            if(transform.position == newPosition)
            {
                if (!_isExpanded) // [Stache TODO]: I am aware it currently does not cover all edge cases, I will do it later
                {
                    _objectsToToggleOn = objectsToToggleOn;
                    _objectsToToggleOff = objectsToToggleOff;

                    _currentExpandedSize = newExpandedSize;
                    _currentExpansionDuration = newExpansionDuration;
                    _currentCollapseDuration = newCollapseDuration;

                    _isExpanding = true;                                        
                    float ratio = _size / newExpandedSize;
                    float equivalentLerpingTimeElapsed = ratio * newExpansionDuration;
                    _lerpTimeStart = TimeLoop.GetSecondsElapsed() - equivalentLerpingTimeElapsed;
                }
                return;
            }

            // Case 2: Different place. Here we need to shrink the potential current one first, then move it.
            if(_objectsToToggleOn != null)
            {
                foreach (var obj in _objectsToToggleOn)
                {
                    obj.SetActive(false); // Was on -> we turn off
                }
            }
            if(_objectsToToggleOff != null)
            {
                foreach (var obj in _objectsToToggleOff)
                {
                    obj.SetActive(true); // Was off -> we turn on
                }
            }
            _objectsToToggleOn = objectsToToggleOn;
            _objectsToToggleOff = objectsToToggleOff;

            _nextPosition = newPosition;
            _nextExpandedSize = newExpandedSize;
            _nextExpansionDuration = newExpansionDuration;
            _nextCollapseDuration = newCollapseDuration;

            if (!_isShrunk) {
                _isShrinking = true;
                float ratio = _size / newExpandedSize;
                float equivalentLerpingTimeElapsed = ratio * newCollapseDuration;
                _lerpTimeStart = TimeLoop.GetSecondsElapsed() - equivalentLerpingTimeElapsed;
            }
            _wishToExpand = true;
        }
        public void Deactivate()
        {
            if (_isShrunk) return;

            _wishToExpand = false;
            _isExpanding = false;
            _isExpanded = false;
            _isShrinking = true;
            float ratio = 1f - (_size / _currentExpandedSize);
            float equivalentLerpingTimeElapsed = ratio * _currentCollapseDuration;
            _lerpTimeStart = TimeLoop.GetSecondsElapsed() - equivalentLerpingTimeElapsed;
        }

        public void Update()
        {
            if (_isShrinking)
            {
                if(_currentCollapseDuration > 0f)
                {
                    _lerpTimeCurrent = (TimeLoop.GetSecondsElapsed() - _lerpTimeStart) / _currentCollapseDuration;
                    _size = Mathf.Lerp(_currentExpandedSize, 0f, _lerpTimeCurrent);
                }
                else
                {
                    _size = 0f;
                }

                if(_size <= 0f)
                {
                    _isShrinking = false;
                    _isShrunk = true;
                }
            }
            else if (_isExpanding)
            {
                _isShrunk = false;

                if (_currentExpansionDuration > 0f)
                {
                    _lerpTimeCurrent = (TimeLoop.GetSecondsElapsed() - _lerpTimeStart) / _currentExpansionDuration;
                    _size = Mathf.Lerp(0f, _currentExpandedSize, _lerpTimeCurrent);
                }
                else
                {
                    _size = _currentExpandedSize;
                }

                if (_size >= _currentExpandedSize)
                {
                    _isExpanding = false;
                    _isExpanded = true;

                    foreach (var obj in _objectsToToggleOn)
                    {
                        obj.SetActive(true);
                    }
                    foreach (var obj in _objectsToToggleOff)
                    {
                        obj.SetActive(false);
                    }
                }
            }
            else if (_wishToExpand)
            {
                transform.position = _nextPosition;
                _currentExpandedSize = _nextExpandedSize;
                _currentExpansionDuration = _nextExpansionDuration;
                _currentCollapseDuration = _nextCollapseDuration;

                _lerpTimeStart = TimeLoop.GetSecondsElapsed();

                _isShrunk = false;
                _isExpanding = true;
                _wishToExpand = false;
            }
        }
    }
}
