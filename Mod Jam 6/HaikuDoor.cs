using UnityEngine;

namespace Mod_Jam_6
{
    public class HaikuDoor : MonoBehaviour
    {
        private SingleInteractionVolume interact;
        private int tryCount;

        [SerializeField]
        private Animator anim;
        [SerializeField]
        private int ID;

        private void Start()
        {
            interact = this.GetRequiredComponent<SingleInteractionVolume>();
            interact.OnPressInteract += OnPressInteract;

            interact.ChangePrompt(ModJam6.NewHorizons.GetTranslationForUI("$PH_TRY_DOOR_PROMPT"));
        }

        private void OnPressInteract()
        {
            if (tryCount < 2)
            {
                anim.SetTrigger("Try");
                tryCount++;
            }
            else
            {
                anim.SetTrigger("Open");
                DoorManager.instance.SyncDoor(ID);
                DoorManager.instance.VoidDoorList[ID].gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}
