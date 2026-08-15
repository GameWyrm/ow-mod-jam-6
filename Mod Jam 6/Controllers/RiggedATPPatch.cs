using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod_Jam_6
{
    [HarmonyPatch]
    public class RiggedATPPatch
    {
        private static bool _hasWarpedToSystem = false;
        private static bool _shouldWarpToSystem = false;
        private const string systemName = "VoidDimension";

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

            TimeLoop.SetSecondsRemaining(1320);
            var warping = ModJam6.NewHorizons.ChangeCurrentStarSystem(systemName);
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
    }
}
