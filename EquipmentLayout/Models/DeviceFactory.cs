using System.Windows;

namespace EquipmentLayout.Models
{
    public class DeviceFactory
    {
        private class DeviceBuild : Device
        {
            public DeviceBuild(DeviceTemplate deviceTemplate, Point position)
                : base(deviceTemplate, (int)position.X, (int)position.Y) { }
        }

        public Device GetDevice(Point position, DeviceTemplate deviceTemplate, bool isIncCount = true)
        {
            if(isIncCount)
                deviceTemplate.Count++;

            return new DeviceBuild(deviceTemplate, position);
        }

    }

}
