using System.Collections.Generic;

namespace TelldusTray
{
    public class Devices
    {
        public List<Device> device { get; set; }
    }
    public class Device
    {
        public int id { get; set; }
        public int clientDeviceId { get; set; }
        public string name { get; set; }
        public int state { get; set; }
        public int? statevalue { get; set; }
        public int methods { get; set; }
        public string type { get; set; }
        public int client { get; set; }
        public string clientName { get; set; }
        public int online { get; set; }
        public int editable { get; set; }
    }

    public class DeviceNames
    {
        public DeviceNames(string Device)
        {
            this.Device = Device;
        }

        public string Device { get; set; }
    }
}
