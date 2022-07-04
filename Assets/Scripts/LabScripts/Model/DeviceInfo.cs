using System;

namespace Model
{
    [Serializable]
    public class DeviceInfo
    {
        public LevelInfo levelInfo;
        public DeviceStatus deviceStatus;

        public DeviceInfo() { }
    }
}