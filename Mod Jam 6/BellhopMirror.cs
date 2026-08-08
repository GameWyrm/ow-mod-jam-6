using System;

namespace Mod_Jam_6
{
    public class BellhopMirror : QuantumObject
    {
        public override bool ChangeQuantumState(bool skipInstantVisibilityCheck)
        {
            return false; // Does not actually move
        }
    }
}
