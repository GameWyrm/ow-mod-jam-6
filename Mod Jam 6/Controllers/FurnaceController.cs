using UnityEngine;

namespace Mod_Jam_6
{
    public class FurnaceController : MonoBehaviour
    {
        public static FurnaceController Instance;

        public int floor; //0-3
        public Animator anim;

        private void Start ()
        {
            Instance = this;
        }

        public void TurnValve(bool clockwise)
        {
            floor = Mathf.Clamp(floor + (clockwise ? 1 : -1), 0, 3);
            anim.SetInteger("CrankValue", floor);
        }
    }
}
