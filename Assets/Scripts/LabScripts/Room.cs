using Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoomController
{
    [Serializable]
    public class Room : MonoBehaviour
    {
        public RoomInfo roomState;
        public List<Device> devices;

        public Device door;

        public Room() { }

        private void Awake()
        {
            GameInfo.room = this;
        }

        public void UpdateRoomInfo()
        {
            if (PlayerPrefs.HasKey(GameInfo.key))
            {
                roomState = Serializator.DeSerialize<RoomInfo>(GameInfo.key);
                Debug.Log(4);
            }

            door.SetDeviceInfo(roomState.deviceInfos[0]);
            for (var i = 1; i < roomState.deviceInfos.Count; i++)
            {
                devices[i - 1].SetDeviceInfo(roomState.deviceInfos[i]);
                devices[i - 1].DeviceStatus = devices[i - 1].DeviceStatus;
            }

            foreach (var device in devices)
                if (device.DeviceStatus == DeviceStatus.nonWorking)
                    return;
            door.DeviceStatus = DeviceStatus.nonWorking;
        }
    }
}