using Mod_Jam_6.Controllers;
using System.Collections;
using UnityEngine;

namespace Mod_Jam_6
{
    public class Door : MonoBehaviour
    {
        public int ID;
        public bool locked;
        public bool voidHotel;

        private void Start()
        {
            if (ID != 0) DoorManager.instance.RegisterDoor(this);
            StartCoroutine(SyncLock());
        }

        public void SyncOpen()
        {
            GetComponent<Animator>().SetTrigger("Open");
            transform.Find("Interaction").gameObject.SetActive(false);
        }

        public IEnumerator SyncLock()
        {
            yield return new WaitForEndOfFrame();
            if (ID != 0 && DoorManager.instance.GetDoorLocked(ID))
            {
                locked = true;
            }
            if (locked)
            {
                GetComponentInChildren<SingleInteractionVolume>().ChangePrompt(ModJam6.Instance.NewHorizons.GetTranslationForUI("$PH_TRY_DOOR_PROMPT"));
                GetComponentInChildren<SingleUseAnimationController>()._animationTrigger = "Try";
                GetComponentInChildren<SingleUseInteract>()._singleUse = false;
            }
            else
            {
                GetComponentInChildren<SingleInteractionVolume>().ChangePrompt(ModJam6.Instance.NewHorizons.GetTranslationForUI("$PH_OPEN_DOOR_PROMPT"));
                GetComponentInChildren<SingleUseAnimationController>()._animationTrigger = "Open";
                GetComponentInChildren<SingleUseInteract>()._singleUse = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = locked ? Color.red : Color.green;
            if (ID == 0) Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}
