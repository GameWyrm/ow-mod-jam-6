using UnityEngine;

namespace Mod_Jam_6
{
    public class PoolLock : MonoBehaviour
    {
        public GameObject lockedDoor;
        public GameObject openDoor;

        private SingleInteractionVolume volume;

        private void Awake()
        {
            volume = this.GetRequiredComponent<SingleInteractionVolume>();
            volume.OnPressInteract += OnPressInteract;
        }

        private void OnPressInteract()
        {
            lockedDoor.SetActive(false);
            openDoor.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}
