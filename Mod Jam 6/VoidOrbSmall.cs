using System.Collections;
using UnityEngine;

namespace Mod_Jam_6
{
    public class VoidOrbSmall : MonoBehaviour
    {
        public FloorEnum floor;
        private void Start()
        {
            if (floor == FloorEnum.FLOOR_3F)
            {
                TimeManager.instance.timeEvents[1].AddListener(Shrink);
            }
            else if (floor == FloorEnum.FLOOR_2F)
            {
                TimeManager.instance.timeEvents[3].AddListener(Shrink);
            }
        }

        private void Shrink()
        {
            StartCoroutine(ShrinkAnim());
        }

        private IEnumerator ShrinkAnim()
        {
            float StartTime = Time.time;

            while (Time.time < StartTime + 1)
            {
                transform.localScale = Vector3.one * (1 - (Time.time - StartTime));
                yield return new WaitForEndOfFrame();
            }

            gameObject.SetActive(false);
        }
    }
}
