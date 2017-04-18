using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelldusTray
{
    public class NewDevice
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Action { get; set; }
    }

    public class NewDeviceList
    {
        public string Saved { get; set; }
    }
}
