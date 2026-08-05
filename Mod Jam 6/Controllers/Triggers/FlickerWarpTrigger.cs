
using System.Collections;
using UnityEngine;

namespace Mod_Jam_6
{
    // A trigger box that, when stepped in, makes your flashlight flicker, closes your eyes, teleports you to a specified transform, then opens your eyes
    public class FlickerWarpTrigger : MonoBehaviour
    {
        [SerializeField]
        private SpawnPoint _warpTarget;

        private bool _isWarping;

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            if (hitCollider.CompareTag("PlayerDetector") && !_isWarping)
            {
                _isWarping = true;
                StartCoroutine(WarpCoroutine());
            }

            if (hitCollider.CompareTag("ProbeDetector"))
            {
                var probe = Locator.GetProbe();
                if (probe != null && probe.IsLaunched())
                {
                    probe.ExternalRetrieve(silent: false);
                }
            }
        }
        private IEnumerator WarpCoroutine()
        {
            // Flashlight flicker
            GlobalMessenger<float, float>.FireEvent("FlickerOffAndOn", 0.5f, 1f);

            // Close eyes
            var cameraEffectController = FindObjectOfType<PlayerCameraEffectController>();
            OWInput.ChangeInputMode(InputMode.None); // stop player input for a while

            cameraEffectController.CloseEyes(Constants.BLINK_CLOSE_ANIM_TIME);
            yield return new WaitForSeconds(Constants.BLINK_CLOSE_ANIM_TIME);
            GlobalMessenger.FireEvent("PlayerBlink");

            yield return new WaitForSeconds(Constants.BLINK_STAY_CLOSED_TIME); // short timer to avoid an actual blink warp, keeping the eyes closed a tiny bit

            // Warp
            var spawner = GameObject.FindGameObjectWithTag("Player").GetRequiredComponent<PlayerSpawner>();
            spawner.DebugWarp(_warpTarget);

            // Open eyes
            cameraEffectController.OpenEyes(Constants.BLINK_OPEN_ANIM_TIME, false);
            yield return new WaitForSeconds(Constants.BLINK_OPEN_ANIM_TIME);
            OWInput.ChangeInputMode(InputMode.Character); // gives the player back input

            _isWarping = false;
        }
    }
}
