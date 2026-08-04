
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
        [SerializeField]
        private OWAudioSource _mainAudio;

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
            StartCoroutine(PlayAnim());
        }

        protected virtual IEnumerator PlayAnim()
        {
            _mainAudio?.PlayOneShot(global::AudioType.NomaiVesselPowerUp, 1f);
            _mainAnimator?.Play("ACTIVATION", 0);
            yield return new WaitForSeconds(2f);
        }
    }
}
