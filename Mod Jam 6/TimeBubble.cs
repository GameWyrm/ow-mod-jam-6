using UnityEngine;

namespace Mod_Jam_6
{
    public class TimeBubble : MonoBehaviour
    {
        public static TimeBubble instance;

        public float size;

        private void Start()
        {
            instance = this;
        }
    }
}
