using UnityEngine;

namespace Mod_Jam_6
{
    public class MaterialManager : MonoBehaviour
    {
        public Material[] materials;
        public float timeProgress = 0.25f;

        private float minProgress = 0.15f;
        private float maxProgress = 0.95f;

        public void UpdateMaterials()
        {
            foreach (Material mat in materials)
            {
                mat.SetFloat("_TimeProgress", timeProgress); // TODO set this to the percentage of time passed
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
