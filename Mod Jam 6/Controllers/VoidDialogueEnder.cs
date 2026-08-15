
using UnityEngine;

namespace Mod_Jam_6
{
    public class VoidDialogueEnder : MonoBehaviour
    {
        [SerializeField]
        private CharacterDialogueTree _dialogue;

        private bool _isConversing = false;

        private void Start()
        {
            if(_dialogue != null)
            {
                GlobalMessenger.AddListener("VoidWarpPlayer", OnVoidWarpPlayer);
                _dialogue.OnStartConversation += OnStartConversation;
                _dialogue.OnEndConversation += OnEndConversation;
            }
        }
        private void OnDestroy()
        {
            if (_dialogue != null)
            {
                GlobalMessenger.RemoveListener("VoidWarpPlayer", OnVoidWarpPlayer);
                _dialogue.OnStartConversation -= OnStartConversation;
                _dialogue.OnEndConversation -= OnEndConversation;
            }
        }

        private void OnStartConversation()
        {
            _isConversing = true;
        }
        private void OnEndConversation()
        {
            _isConversing = false;
        }
        private void OnVoidWarpPlayer()
        {
            if (_isConversing)
            {
                _dialogue.EndConversation();
                _isConversing = false;
            }
        }
    }
}
