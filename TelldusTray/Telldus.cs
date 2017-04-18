using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TelldusTray
{
    public class Telldus
    {
        public static Dictionary<string, int> ids;

        private const string PublicKey = "";
        private const string PrivateKey = "";
        private const string Token = "";
        private const string TokenSecret = "";

        private const string BaseUrl = "http://api.telldus.com/";

        public static void DeviceAction(int id, string action)
        {
            var client = new RestClient(BaseUrl);
            client.Authenticator = OAuth1Authenticator.ForProtectedResource(PublicKey, PrivateKey, Token, TokenSecret);

            var request = new RestRequest("json/device/" + action, Method.POST);
            request.AddParameter("id", id);

            var response = client.Execute(request);
        }

        public static List<Device> GetDevices()
        {       
            var client = new RestClient(BaseUrl);
            client.Authenticator = OAuth1Authenticator.ForProtectedResource(PublicKey, PrivateKey, Token, TokenSecret);

            var request = new RestRequest("json/devices/list", Method.GET);
            var response = client.Execute(request);
        
            Devices deviceList = JsonConvert.DeserializeObject<Devices>(response.Content);
            deviceList.device.RemoveAll(x => x.name.ElementAt(0) == 'S');
                   
            return deviceList.device;
        }

        public static List<DeviceNames> GetNames()
        {
            List<DeviceNames> names = new List<DeviceNames>();
            ids = new Dictionary<string, int>();

            var list = GetDevices();
            foreach(var item in list)
            {
                names.Add(new DeviceNames(item.name));
                ids[item.name] = item.id;
            }

            return names;
        }

        public static int GetId(string name)
        {
            return ids[name];
        }
    }
}
