using UnityEngine;

namespace Mod_Jam_6
{
    public class EnableComponent : MonoBehaviour
    {
        public MonoBehaviour componentToEnable;

        private void Update()
        {
            componentToEnable.enabled = true;
        }
    }
}
