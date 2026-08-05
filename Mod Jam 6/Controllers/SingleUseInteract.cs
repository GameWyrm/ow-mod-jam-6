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
        private string _prompt;
        [SerializeField]
        private bool _singleUse = true;

        private bool _hasBeenUsed = false;

        private void Start()
        {
            if (_interactReceiver == null) _interactReceiver = GetComponent<InteractReceiver>();
            if (_interactReceiver != null)
            {
                _interactReceiver.OnPressInteract += OnPressInteract;
                _interactReceiver.ChangePrompt(_prompt);
            }
        }
        private void OnDestroy()
        {
            if (_interactReceiver != null)
            {
                _interactReceiver.OnPressInteract -= OnPressInteract;
            }
        }

        private void OnPressInteract()
        {
            if (_hasBeenUsed) return; // Single use

            OnSingleUseActivation.Invoke();

            if (_singleUse)
            {
                _hasBeenUsed = true;
                gameObject.SetActive(false);
            }
        }
    }
}
