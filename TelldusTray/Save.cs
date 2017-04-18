using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TelldusTray
{
    public class Save
    {
        private static string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\Saves.txt");

        private static List<NewDevice> newList;

        public static void SaveDevice(string name, int id, string action)
        {
            string device = name + "," + id + "," + action + Environment.NewLine;

            File.AppendAllText(path, device);
        }

        public static List<NewDevice> GetDevices()
        {
            List<NewDevice> list = new List<NewDevice>();
            string[] files = File.ReadAllLines(path);
            foreach(string s in files)
            {
                NewDevice d = new NewDevice();
                string[] arr = s.Split(',');
                d.Name = arr[0];
                d.Id = Int32.Parse(arr[1]);
                d.Action = arr[2];
                list.Add(d);
            }

            newList = new List<NewDevice>(list);

            return list;
        }

        public static List<NewDeviceList> GetNoIdDevices()
        {
            List<NewDeviceList> list = new List<NewDeviceList>();
            
            foreach(var item in newList)
            {
                NewDeviceList d = new NewDeviceList();
                d.Saved = item.Name;   
                list.Add(d);
            }

            return list;
        }

        public static void RemoveDevice(int index)
        {
            NewDevice nd = newList[index];

            var tempFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data/SavesTmp.txt");
            var fileName = "Data/Saves.txt";
            var remove = nd.Name + "," + nd.Id + "," + nd.Action;
            var linesToKeep = File.ReadLines(path).Where(l => l != remove);

            File.WriteAllLines(tempFile, linesToKeep);
            File.Delete(fileName);
            File.Move(tempFile, path);
        }
    }
}
