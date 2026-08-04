
using System.Collections;
using UnityEngine;

namespace Mod_Jam_6
{
    public class SingleUseInteract : MonoBehaviour
    {
        public delegate void SingleUseActivationEvent();
        public SingleUseActivationEvent OnSingleUseActivation;

        [SerializeField]
        private InteractReceiver _interactReceiver;
        [SerializeField]
        private Animator _interactAnimator;
        [SerializeField]
        private OWAudioSource _interactAudio;

        [SerializeField]
        private AudioType _interactAudioType = global::AudioType.GearRotate_Heavy; // Placeholder just to make sure it works, change to whatever you fancy in unity
        [SerializeField]
        private float _interactAudioDuration = 1f; // Also placeholder

        private bool _hasBeenUsed = false;

        private void Start()
        {
            if (_interactReceiver != null)
            {
                _interactReceiver.OnPressInteract += OnPressInteract;
            }
        }
        private void OnDestroy()
        {
            if (_interactReceiver != null)
            {
                _interactReceiver.OnPressInteract -= OnPressInteract;
            }
        }

        private IEnumerator PlayInteract() // Just the beepboop and slight movement of the button press
        {
            _interactAudio?.PlayOneShot(_interactAudioType, _interactAudioDuration);
            _interactAnimator?.Play("ACTIVATION", 0);
            yield return new WaitForSeconds(1f);
        }

        private void OnPressInteract()
        {
            if (_hasBeenUsed) return; // Single use

            _hasBeenUsed = true;
            StartCoroutine(PlayInteract());
            OnSingleUseActivation.Invoke();
        }
    }
}
