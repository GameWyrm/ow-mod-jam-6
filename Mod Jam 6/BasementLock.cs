using UnityEngine;

namespace Mod_Jam_6
{
    public class BasementLock : MonoBehaviour
    {
        public Animator anim;

        private SingleInteractionVolume interact;
        

        private void Start()
        {
            interact = this.GetRequiredComponent<SingleInteractionVolume>();
            interact.OnPressInteract += OnPressInteract;

            interact.ChangePrompt(ModJam6.Instance.NewHorizons.GetTranslationForUI("$PH_OPEN_DOOR_PROMPT"));
        }

        private void OnPressInteract()
        {
            if (FurnaceController.Instance.floor == 0 && BreakerBox.instance.repaired)
            {
                anim.SetTrigger("Open");
                gameObject.SetActive(false);
            }
            else
            {
                anim.SetTrigger("Try");
            }
        }
    }
}
