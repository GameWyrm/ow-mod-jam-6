using UnityEngine;

namespace Mod_Jam_6
{
    public class RiggedATPCore : WarpCoreItem
    {
        [SerializeField]
        private GameObject _warpPlatformOrBlackHoleIDK;

        public override void PickUpItem(Transform holdTranform)
        {
            base.PickUpItem(holdTranform);
            ToggleWarp(isRigged: true);
        }
        public override void DropItem(Vector3 position, Vector3 normal, Transform parent, Sector sector, IItemDropTarget customDropTarget)
        {
            base.DropItem(position, normal, parent, sector, customDropTarget);
            ToggleWarp(isRigged: false);
        }
        public override void SocketItem(Transform socketTransform, Sector sector)
        {
            base.SocketItem(socketTransform, sector);
            ToggleWarp(isRigged: false);
        }

        private void ToggleWarp(bool isRigged)
        {
            // TODO
        }
    }
}
