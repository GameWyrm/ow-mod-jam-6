using UnityEngine;

namespace Mod_Jam_6
{
    public class MaterialManager : MonoBehaviour
    {
        public static MaterialManager instance;

        public Material[] materials;
        public float timeProgress = 0.25f;

        private float minProgress = 0.15f;
        private float maxProgress = 0.95f;

        private void Start ()
        {
            instance = this;
        }

        public void UpdateMaterials()
        {
            foreach (Material mat in materials)
            {
                mat.SetFloat("_TimeProgress", timeProgress);
                mat.SetVector("_BubblePosition", TimeBubble.instance.gameObject.transform.position);
                mat.SetFloat("_BubbleDistance", TimeBubble.instance.size);
            }
        }

        private void Update()
        {
            // There are 16 minutes in our loop
            timeProgress = Mathf.InverseLerp(0, (16 * 60), TimeLoop.GetSecondsElapsed());
            timeProgress = Mathf.Clamp(timeProgress, minProgress, maxProgress);

            UpdateMaterials();
        }
    }
}
