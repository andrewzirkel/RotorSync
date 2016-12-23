using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotorSync
{
    class RS
    {
        //properties
        private string deviceID;
        private RSHDHomeRunDevice hdhr;
        public string channel { get; private set; }
        public string signalStrength { get; private set; }
        public string signalQuality { get; private set; }
        public string symbolQuality { get; private set; }

        //constructor
        public RS(string id)
        {
            deviceID = id;
            hdhr = new RSHDHomeRunDevice(deviceID);
        }

        public bool DoSync()
        {
            channel = hdhr.RSHDHomeRunGet("/tuner0/channel");
            if (channel.Equals("none")) return false;
            string statusRaw = hdhr.RSHDHomeRunGet("/tuner0/status");
            var status = statusRaw.Split(new[] {' '})
                .Select(part => part.Split('='))
                .ToDictionary(split => split[0], split => split[1]);
            signalStrength = status["ss"];
            signalQuality = status["snq"];
            symbolQuality = status["seq"];
            if (symbolQuality.Equals("100")) return false;
            return true;
        }
    }
}
