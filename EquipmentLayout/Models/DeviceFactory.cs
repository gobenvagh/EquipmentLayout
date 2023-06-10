using System.Windows;

namespace EquipmentLayout.Models
{
    public class DeviceFactory
    {
        private class DeviceBuild : Device
        {
            public DeviceBuild(DeviceTemplate deviceTemplate, Point position, string name)
                : base(deviceTemplate, position, name) { }
        }

        public Device GetDevice(Point position, DeviceTemplate deviceTemplate, string name)
        {
            return new DeviceBuild(deviceTemplate, position, name);
        }
    }
}
