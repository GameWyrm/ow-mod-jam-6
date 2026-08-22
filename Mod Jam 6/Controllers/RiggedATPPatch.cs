using HarmonyLib;
using UnityEngine;

namespace Mod_Jam_6
{
    [HarmonyPatch]
    public class RiggedATPPatch
    {
        public static bool hasWarpedToSystem = false;

        private static bool _shouldWarpToSystem = false;

        private const string systemName = "VoidDimension";
        private const int _timeloopLength = 1320;
        private const string _atpPath = "Sector_TowerTwin/Sector_TimeLoopInterior";

        private static AudioSignal _signal;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TimeLoopCoreController), nameof(TimeLoopCoreController.OnSocketablePlaced))]
        public static void TimeLoopCoreController_OnSocketablePlaced_Postfix(OWItem socketableItem)
        {
            if (IsATPCore(socketableItem))
            {
                _shouldWarpToSystem = false;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TimeLoopCoreController), nameof(TimeLoopCoreController.OnSocketableRemoved))]
        public static void TimeLoopCoreController_OnSocketableRemoved_Prefix(OWItem socketableItem)
        {
            //_signal = Locator.GetRootTransform()?.Find(_atpPath)?.GetComponentInChildren<AudioSignal>();
            var signals = Resources.FindObjectsOfTypeAll<AudioSignal>();
            foreach (var signal in signals)
            {
                if (signal.gameObject.name == "PH_WarpCoreSignal")
                {
                    _signal = signal;
                    break;
                }
            }

            if (_signal == null)
            {
                ModJam6.Instance.ModHelper.Console.WriteLine($"DID NOT FIND SIGNAL", OWML.Common.MessageType.Error);
                return;
            }

            var frequency = _signal.GetFrequency();
            var name = _signal.GetName();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TimeLoopCoreController), nameof(TimeLoopCoreController.OnSocketableRemoved))]
        public static void TimeLoopCoreController_OnSocketableRemoved_Postfix(OWItem socketableItem)
        {
            if (IsATPCore(socketableItem))
            {
                _shouldWarpToSystem = true;
            }
        }

        private static bool IsATPCore(OWItem item)
        {
            return (item.GetType() == typeof(WarpCoreItem) && ((WarpCoreItem)item).GetWarpCoreType() == WarpCoreType.Vessel); // Same check as in game
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NomaiWarpPlatform), nameof(NomaiWarpPlatform.TransmitWarpedBody))]
        public static bool NomaiWarpPlatform_TransmitWarpedBody_Prefix()
        {
            if(hasWarpedToSystem) { return false; } // Empty method call while warping

            if(!_shouldWarpToSystem) { return true; } // No patch if shouldn't warp

            TimeLoop.SetSecondsRemaining(_timeloopLength);
            ForceIdentifySignal();
            var warping = ModJam6.NewHorizons.ChangeCurrentStarSystem(systemName);
            if (warping)
            {
                PlayerData.SetPersistentCondition("PH_PLAY_MOD", true);
                hasWarpedToSystem = true;
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void ForceIdentifySignal()
        {
            if(_signal == null)
            {
                ModJam6.Instance.ModHelper.Console.WriteLine($"DID NOT FIND SIGNAL", OWML.Common.MessageType.Error);
                return;
            }

            var frequency = _signal.GetFrequency();
            var name = _signal.GetName();

            //ModJam6.Instance.ModHelper.Console.WriteLine($"Found ({frequency} - {name})", OWML.Common.MessageType.Error);

            if(!PlayerData.KnowsFrequency(frequency))
            {
                PlayerData.LearnFrequency(frequency);
            }
            if (!PlayerData.KnowsSignal(name))
            {
                PlayerData.LearnSignal(name);
            }
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Flashback), nameof(Flashback.OnTriggerFlashback))]
        public static bool OnTriggerFlashback(Flashback __instance)
        {
            if (ModJam6.NewHorizons.GetCurrentStarSystem() == "VoidDimension")
            {
                if (PlayerData.GetPersistentCondition("GAME_OVER_LAST_SAVE"))
                {
                    PlayerData.SetPersistentCondition("GAME_OVER_LAST_SAVE", false);
                }
                CenterOfTheUniverse.DeactivateUniverse();
                Locator.GetActiveCamera().enabled = false;
                __instance.transform.position = Vector3.zero;
                __instance.ResetEffects();
                __instance._flashbackCamera.clearFlags = CameraClearFlags.Color;
                __instance._flashbackCamera.enabled = true;
                GlobalMessenger<OWCamera>.FireEvent("SwitchActiveCamera", __instance._flashbackCamera);
                __instance._audioListener.enabled = true;
                __instance._screenTransform.gameObject.SetActive(true);
                __instance._maskTransform.gameObject.SetActive(true);
                __instance._forwardStreams.SetActive(true);
                int numSnapshots = __instance._flashbackRecorder.GetNumSnapshots();
                float num = 0f;
                __instance._imageDisplayTimes = new float[numSnapshots];
                for (int i = 0; i < numSnapshots; i++)
                {
                    num += Mathf.Max(0.6f * Mathf.Pow(0.9f, (float)i), 0.06f);
                    __instance._imageDisplayTimes[numSnapshots - 1 - i] = num;
                }
                __instance._snapshotIndex = numSnapshots - 1;
                num += 1f;
                /**
                DeathType deathType = Locator.GetDeathManager().GetDeathType();
                if (deathType == DeathType.Supernova || deathType == DeathType.Energy || deathType == DeathType.Lava || deathType == DeathType.DreamExplosion)
                {
                    __instance._flashbackStartDelay = 3f;
                    __instance._whiteFadeAnimator.gameObject.SetActive(true);
                    __instance._whiteFadeAnimator.SetImmediate(1f);
                    __instance._whiteFadeAnimator.AnimateTo(0f, Vector3.one, 3f, __instance._whiteFadeCurve, false);
                }
                **/
                GameObject.Find("FlashbackCamera/Mask").SetActive(false);
                __instance._flashbackStartDelay = 2f;
                GameObject.Find("HelmentRoot").GetComponent<Animator>().Play("MaskStart");
                __instance._flashbackTimer = new Timer(__instance._flashbackStartDelay + __instance._playbackDelay + num);
                __instance._updateFlashback = true;
                __instance._screenTransform.SetLocalPositionZ(__instance._screenStartDist);
                __instance._screenRenderer.material.color = new Color(1f, 1f, 1f, 0f);
                __instance.SetFlashbackImage(__instance._snapshotIndex);
                __instance._maskTransform.SetLocalPositionZ(__instance._maskStartDist);
                GlobalMessenger.FireEvent("FlashbackStart");
                LoadManager.ReloadSceneAsync(false, false);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
