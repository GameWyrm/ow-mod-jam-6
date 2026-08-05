
using UnityEngine;

namespace Mod_Jam_6
{
    public class TimeBubbleActivator : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _timeBubbleOffset = Vector3.zero;
        [SerializeField]
        private float _timeBubbleSize = 5;
        [SerializeField]
        private float _timeBubbleExpansionDuration = 0.5f; // How fast the bubble will expand to its max size
        [SerializeField]
        private float _timeBubbleCollapseDuration = 0.5f; // How fast the bubble will collapse (assuming from its max size)

        [SerializeField]
        private bool _isActive;
        public bool IsActive => _isActive;

        public void SetActive(bool isActive)
        {
            if (_isActive == isActive) return;

            _isActive = isActive;
            if (isActive)
            {
                TimeBubble.instance.TryActivate(transform.position + _timeBubbleOffset, _timeBubbleSize, _timeBubbleExpansionDuration, _timeBubbleCollapseDuration);
            }
            else
            {
                TimeBubble.instance.Deactivate();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + _timeBubbleOffset, _timeBubbleSize);
        }
    }
}
