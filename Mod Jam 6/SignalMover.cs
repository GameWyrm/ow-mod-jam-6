using UnityEngine;
using System.Collections.Generic;

namespace Mod_Jam_6
{
    public class SignalMover : MonoBehaviour
    {
        public static Dictionary<Color, SignalMover> HotelInstance = new();
        public static Dictionary<Color, SignalMover> VoidInstance;

        public FloorEnum floor;
        public Color color;
        public GameObject signalObject;
        public bool isVoid = false;

        private void Awake()
        {
            if (isVoid)
            {
                VoidInstance.Add(color, this);
            }
            else
            {
                HotelInstance.Add(color, this);
            }
        }

        private void Start()
        {
            if (isVoid && signalObject != null)
            {
                signalObject.transform.SetParent(HotelInstance[color].transform, false);
                if (floor == FloorEnum.FLOOR_3F)
                {
                    TimeManager.instance.timeEvents[1].AddListener(OnFloorConsumed);
                }
                if (floor == FloorEnum.FLOOR_2F)
                {
                    TimeManager.instance.timeEvents[3].AddListener(OnFloorConsumed);
                }
            }
        }

        private void OnDestroy()
        {
            HotelInstance.Clear();
            VoidInstance.Clear();
        }

        private void OnFloorConsumed()
        {
            signalObject.transform.SetParent(VoidInstance[color].transform, false);
        }

        public enum Color
        {
            Blue,
            Green,
            Purple
        }
    }
}
