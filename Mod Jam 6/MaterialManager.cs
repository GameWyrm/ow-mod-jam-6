using UnityEngine;

namespace Mod_Jam_6
{
    public class MaterialManager : MonoBehaviour
    {
        public Material[] materials;

        public void UpdateMaterials()
        {
            foreach (Material mat in materials)
            {
                mat.SetFloat("_TimeProgress", 0.1f); // TODO set this to the percentage of time passed
                mat.SetVector("_BubblePosition", TimeBubble.instance.gameObject.transform.position);
                mat.SetFloat("_BubbleDistance", TimeBubble.instance.size);
            }
        }

        private void Update()
        {
            UpdateMaterials();
        }
    }
}
