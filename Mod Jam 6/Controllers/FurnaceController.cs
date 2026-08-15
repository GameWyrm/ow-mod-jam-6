using UnityEngine;

namespace Mod_Jam_6
{
    public class FurnaceController : MonoBehaviour
    {
        public static FurnaceController Instance;
        public static FurnaceController FakeInstance;

        public int floor; //0-3
        public Animator anim;
        public bool realInstance = false;

        private void Start ()
        {
            if (realInstance) Instance = this;
            else FakeInstance = this;
        }

        public void TurnValve(bool clockwise)
        {
            floor = Mathf.Clamp(floor + (clockwise ? 1 : -1), 0, 3);
            anim.SetInteger("CrankValue", floor);
            FakeInstance.anim.SetInteger("CrankValue", floor);
        }
    }
}
