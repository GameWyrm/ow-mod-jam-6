using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Mod_Jam_6
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager instance;

        public GameObject voidBox;
        public GameObject voidTrigger;
        public GameObject voidBlock;

        public List<int> eventTimes = new();
        public float warningTransitionTime = 3f;
        public float floorAbsorbTime = 10f;

        public List<float> voidBoxPositions = new();

        private int currentEvent = -1;

        public int CurrentEvent
        {
            private set { currentEvent = value; }
            get { return currentEvent; }
        }

        private void Start()
        {
            instance = this;
        }

        private void Update()
        {
            if (currentEvent < eventTimes.Count - 1 && TimeLoop.GetSecondsElapsed() > eventTimes[currentEvent + 1])
            {
                currentEvent++;
                RunEvent();
            }
        }

        private void RunEvent()
        {
            ModJam6.Log($"Starting event {currentEvent}");
            switch (currentEvent)
            {
                case 0:
                    StartCoroutine(MoveVoid(warningTransitionTime, false));
                    break;
                case 1:
                    StartCoroutine(MoveVoid(floorAbsorbTime, true));
                    break;
                case 2:
                    StartCoroutine(MoveVoid(warningTransitionTime, false));
                    break;
                case 3:
                    StartCoroutine(MoveVoid(floorAbsorbTime, true));
                    break;
                case 4:
                    StartCoroutine(MoveVoid(warningTransitionTime, false));
                    break;
                case 5:
                    StartCoroutine(MoveVoid(floorAbsorbTime, true));
                    break;
                default:
                    break;
            }
        }

        private IEnumerator MoveVoid(float transitionTime, bool toggleColliders)
        {
            Vector3 initialPosition = voidBox.transform.localPosition;
            Vector3 targetPosition = new(initialPosition.x, voidBoxPositions[currentEvent], initialPosition.z);
            float startTime = Time.time;
            float endTime = Time.time + transitionTime;
            ModJam6.Log($"Initial: {initialPosition.ToString()}, Target: {targetPosition.ToString()}, Start: {startTime}, End: {endTime}");
            if (toggleColliders)
            {
                voidTrigger.SetActive(true);
                voidBlock.SetActive(false);
            }
            while (Time.time < endTime)
            {
                float t = Mathf.InverseLerp(startTime, endTime, Time.time);
                voidBox.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);

                yield return new WaitForEndOfFrame();
            }

            if (Time.time >= endTime)
            {
                voidBox.transform.localPosition = targetPosition;
            }

            if (toggleColliders)
            {
                voidTrigger.SetActive(false);
                voidBlock.SetActive(true);
            }

        }

        private void OnDrawGizmos()
        {
            if (voidBox == null) return;
            Gizmos.color = Color.magenta;
            for (int i = 0; i < voidBoxPositions.Count; i++)
            {
                if (i != voidBoxPositions.Count - 1)
                {
                    Gizmos.DrawLine(GetLocalBoxPosition(i), GetLocalBoxPosition(i + 1));
                    Gizmos.DrawLine(transform.position + new Vector3(50, voidBoxPositions[i], 50), transform.position + new Vector3(50, voidBoxPositions[i], -50));
                    Gizmos.DrawLine(transform.position + new Vector3(50, voidBoxPositions[i], 50), transform.position + new Vector3(-50, voidBoxPositions[i], 50));
                    Gizmos.DrawLine(transform.position + new Vector3(-50, voidBoxPositions[i], -50), transform.position + new Vector3(50, voidBoxPositions[i], -50));
                    Gizmos.DrawLine(transform.position + new Vector3(-50, voidBoxPositions[i], -50), transform.position + new Vector3(-50, voidBoxPositions[i], 50));
                }
            }
        }

        private Vector3 GetLocalBoxPosition(int id)
        {
            return transform.position + new Vector3(0, voidBoxPositions[id], 0);
        }
    }
}
