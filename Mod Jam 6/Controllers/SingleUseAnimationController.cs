
using System.Collections;
using UnityEngine;

namespace Mod_Jam_6.Controllers
{
    // Base of an animation controller with virtual methods. create another class and override the PlayAnim() for more custom animations
    public class SingleUseAnimationController : MonoBehaviour
    {
        [SerializeField]
        private SingleUseInteract _singleUseInteract;

        [SerializeField]
        private Animator _mainAnimator;

        [Tooltip("Trigger on the animation that will be set")]
        public string _animationTrigger;

        protected virtual void Start()
        {
            if (_singleUseInteract != null)
            {
                _singleUseInteract.OnSingleUseActivation += OnActivation;
            }
        }
        protected virtual void OnDestroy()
        {
            if (_singleUseInteract != null)
            {
                _singleUseInteract.OnSingleUseActivation -= OnActivation;
            }
        }

        protected virtual void OnActivation()
        {
            _mainAnimator?.SetTrigger(_animationTrigger);
        }
    }
}
