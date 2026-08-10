
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Jam_6
{
    public class FloorVoidData
    {
        public FloorEnum floor;
        public float voidingTime;

        public FloorVoidData(FloorEnum floor, float voidingTime)
        {
            this.floor = floor;
            this.voidingTime = voidingTime;
        }
        public int CompareTo(FloorVoidData compareData)
        {
            // A null value means that this object is greater.
            if (compareData == null)
                return 1;

            else
                return this.voidingTime.CompareTo(compareData.voidingTime);
        }
    }


    public class FloorVoidManager : MonoBehaviour
    {
        public static FloorVoidManager instance;

        public delegate void FloorVoidingEvent(FloorEnum floor);
        public FloorVoidingEvent OnFloorVoiding;

        private List<FloorVoidData> _voidFloorData = new List<FloorVoidData>();


        private void Start()
        {
            instance = this;

            _voidFloorData.Add(new FloorVoidData(FloorEnum.FLOOR_1F, Constants.FLOORVOIDING_TIME_1F));
            _voidFloorData.Add(new FloorVoidData(FloorEnum.FLOOR_2F, Constants.FLOORVOIDING_TIME_2F));
            _voidFloorData.Add(new FloorVoidData(FloorEnum.FLOOR_3F, Constants.FLOORVOIDING_TIME_3F));
            _voidFloorData.Add(new FloorVoidData(FloorEnum.FLOOR_4F, Constants.FLOORVOIDING_TIME_4F));
            _voidFloorData.Add(new FloorVoidData(FloorEnum.FLOOR_5F, Constants.FLOORVOIDING_TIME_5F));
            _voidFloorData.Add(new FloorVoidData(FloorEnum.FLOOR_1B, Constants.FLOORVOIDING_TIME_1B));
            _voidFloorData.Add(new FloorVoidData(FloorEnum.FLOOR_2B, Constants.FLOORVOIDING_TIME_2B));
            _voidFloorData.Add(new FloorVoidData(FloorEnum.FLOOR_3B, Constants.FLOORVOIDING_TIME_3B));
            _voidFloorData.Sort(); // Sorted by ascending time to void
        }

        private void Update()
        {
            if(_voidFloorData.Count == 0) // disable update when all floors are voided
            {
                base.enabled = false;
                return;
            }

            var diff = _voidFloorData[0].voidingTime - TimeLoop.GetSecondsElapsed();
            if(diff <= 0f)
            {
                OnFloorVoiding?.Invoke(_voidFloorData[0].floor);
                _voidFloorData.RemoveAt(0);
            }
            else
            {
                // Performance: Maybe have the update sleep until next floor ready for voiding? (should be diff)
            }
        }
    }
}
