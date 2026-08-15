using UnityEngine;

namespace Mod_Jam_6
{
    public class IceController : MonoBehaviour
    {
        public CapsuleCollider talkVolume;
        public Animator anim;

        private void Start()
        {
            talkVolume.enabled = false;
            TimeManager.instance.timeEvents[3].AddListener(TryMelt);
        }

        private void TryMelt()
        {
            if (FurnaceController.Instance.floor == 3)
            {
                talkVolume.enabled = true;
                anim.SetTrigger("Melt");
            }
        }
    }
}
