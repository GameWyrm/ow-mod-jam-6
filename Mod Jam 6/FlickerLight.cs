using System.Collections;
using UnityEngine;

namespace Mod_Jam_6
{
    public class FlickerLight : MonoBehaviour
    {
        public float maxTime = 1;
        public float minTime = 0.1f;
        public GameObject light;
        public Material litMaterial;
        public Material unlitMaterial;
        public MeshRenderer rend;

        private float waitTime;
        private bool lit = true;

        private void Start ()
        {
            waitTime = Random.Range(minTime, maxTime);
            StartCoroutine(Flicker());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private IEnumerator Flicker()
        {
            while (true)
            {
                yield return new WaitForSeconds(waitTime);
                lit = !lit;
                light.SetActive(lit);
                if (lit)
                {
                    rend.material = litMaterial;
                }
                else
                {
                    rend.material = unlitMaterial;
                }
                waitTime = Random.Range(minTime, maxTime);
            }
        }
    }
}
