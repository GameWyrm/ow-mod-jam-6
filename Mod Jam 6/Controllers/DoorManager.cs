using UnityEngine;
using System.Collections.Generic;

namespace Mod_Jam_6
{
    public class DoorManager : MonoBehaviour
    {
        public Dictionary<int, Door> HotelDoorList;
        public Dictionary<int, Door> VoidDoorList;
        public static DoorManager instance;

        private void Awake()
        {
            HotelDoorList = new Dictionary<int, Door>();
            VoidDoorList = new Dictionary<int, Door>();
            instance = this;
        }

        public void SyncDoor(int Door)
        {
            if (VoidDoorList.ContainsKey(Door) && VoidDoorList[Door] != null && !HotelDoorList[Door].locked)
            {
                VoidDoorList[Door].SyncOpen();
            }
        }

        public void RegisterDoor(Door door)
        {
            if (door.voidHotel)
            {
                if (!VoidDoorList.ContainsKey(door.ID))
                {
                    VoidDoorList.Add(door.ID, door);
                }
                else
                {
                    ModJam6.LogError($"Void Door {door.ID} already registered!");
                }
            }
            else
            {
                if (!HotelDoorList.ContainsKey(door.ID))
                {
                    HotelDoorList.Add(door.ID, door);
                }
                else
                {
                    ModJam6.LogError($"Hotel Door {door.ID} already registered!");
                }
            }
        }

        public bool GetDoorLocked(int Door)
        {
            if (HotelDoorList.ContainsKey(Door))
            {
                return HotelDoorList[Door].locked;
            }
            else
            {
                ModJam6.LogError($"Requesting the lock state of {Door} but couldn't find it in the hotel door list!");
                return false;
            }
        }
    }
}
