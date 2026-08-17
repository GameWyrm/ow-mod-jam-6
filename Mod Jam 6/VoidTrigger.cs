using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mod_Jam_6
{
    public class VoidTrigger : MonoBehaviour
    {
        public List<string> spawnPoints = new();

        private void OnTriggerEnter(Collider other)
        {
            ModJam6.Log($"A collider called {other.name} entered.");
            if (other.name.Contains("Player")) StartCoroutine(WarpPlayer());
        }

        private IEnumerator WarpPlayer()
        {
            ModJam6.Log("Warping player!");

            GlobalMessenger.FireEvent("VoidWarpPlayer");

            SpawnPoint warpTarget = null;
            string spawn = "";

            if (TimeManager.instance.CurrentEvent >= 5) spawn = spawnPoints[0];
            else if (TimeManager.instance.CurrentEvent >= 3) spawn = spawnPoints[1];
            else if (TimeManager.instance.CurrentEvent >= 1) spawn = spawnPoints[2];

            warpTarget = GameObject.Find(spawn).GetComponent<SpawnPoint>();

            var cameraEffectController = FindObjectOfType<PlayerCameraEffectController>();
            OWInput.ChangeInputMode(InputMode.None);

            cameraEffectController.CloseEyes(Constants.BLINK_CLOSE_ANIM_TIME);
            yield return new WaitForSeconds(Constants.BLINK_CLOSE_ANIM_TIME);
            GlobalMessenger.FireEvent("PlayerBlink");

            yield return new WaitForSeconds(Constants.BLINK_STAY_CLOSED_TIME);

            var spawner = GameObject.FindGameObjectWithTag("Player").GetRequiredComponent<PlayerSpawner>();
            spawner.DebugWarp(warpTarget);
            OWInput.ChangeInputMode(InputMode.Character);

            PlayerData.SetPersistentCondition("PH_FOUND_SOMETHING_P", true);
            PlayerData.SetPersistentCondition("PH_FOUND_VOID_P", true);

            cameraEffectController.OpenEyes(Constants.BLINK_OPEN_ANIM_TIME, false);
            yield return new WaitForSeconds(Constants.BLINK_OPEN_ANIM_TIME);
        }
    }
}
