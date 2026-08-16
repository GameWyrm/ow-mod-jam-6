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
     
        public string _prompt;

        public bool _singleUse = true;

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

            // Door hack
            if (GetComponentInParent<Door>() != null) DoorManager.instance.SyncDoor(GetComponentInParent<Door>().ID);

            if (_singleUse)
            {
                _hasBeenUsed = true;
                gameObject.SetActive(false);
            }
        }
    }
}
