using HarmonyLib;

namespace Mod_Jam_6
{
    [HarmonyPatch]
    public class RiggedATPPatch
    {
        private static bool _hasWarpedToSystem = false;
        private static bool _shouldWarpToSystem = false;

        private const string systemName = "VoidDimension";
        private const int _timeloopLength = 1320;
        private const string _atpPath = "Sector_TowerTwin/Sector_TimeLoopInterior";

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
            if(_hasWarpedToSystem) { return false; } // Empty method call while warping

            if(!_shouldWarpToSystem) { return true; } // No patch if shouldn't warp

            TimeLoop.SetSecondsRemaining(_timeloopLength);
            ForceIdentifySignal();
            var warping = ModJam6.Instance.NewHorizons.ChangeCurrentStarSystem(systemName);
            if (warping)
            {
                _hasWarpedToSystem = true;
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void ForceIdentifySignal()
        {
            var warpcoreSignal = Locator.GetRootTransform()?.Find(_atpPath)?.GetComponentInChildren<AudioSignal>();
            if(warpcoreSignal == null)
            {
                ModJam6.Instance.ModHelper.Console.WriteLine($"DID NOT FIND SIGNAL", OWML.Common.MessageType.Error);
                return;
            }

            var frequency = warpcoreSignal.GetFrequency();
            var name = warpcoreSignal.GetName();

            ModJam6.Instance.ModHelper.Console.WriteLine($"Found ({frequency} - {name})", OWML.Common.MessageType.Error);

            if(!PlayerData.KnowsFrequency(frequency))
            {
                PlayerData.LearnFrequency(frequency);
            }
            if (!PlayerData.KnowsSignal(name))
            {
                PlayerData.LearnSignal(name);
            }
        }
    }
}
