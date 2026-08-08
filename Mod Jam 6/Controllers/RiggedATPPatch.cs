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

        public static void SetShouldWarpToSystem(bool newValue)
        {
            _shouldWarpToSystem = newValue;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NomaiWarpPlatform), nameof(NomaiWarpPlatform.TransmitWarpedBody))]
        public static bool NomaiWarpPlatform_TransmitWarpedBody_Prefix()
        {
            if(_hasWarpedToSystem) { return false; } // Empty method call while warping

            if(!_shouldWarpToSystem) { return true; } // No patch if shouldn't warp

            var warping = ModJam6.Instance.NewHorizons.ChangeCurrentStarSystem("NEW SYSTEM");
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
