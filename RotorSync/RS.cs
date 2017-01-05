using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RotorSync
{
    class RS
    {
        //properties
        public string deviceID;
        private RSHDHomeRunDevice hdhr;
        private RSData rsdata;

        //keeping track of timeouts
        private int badSignalCounter;

        public string channel { get; private set; }
        public string signalStrength { get; private set; }
        public string signalQuality { get; private set; }
        public string symbolQuality { get; private set; }
        public long currentAzimuth { get; private set; }

        //constructor
        public RS(string id)
        {
            deviceID = id;
            hdhr = new RSHDHomeRunDevice(deviceID);
            rsdata = new RSData("C:\\Code\\RotorSync\\RotorSync\\RSDatabase.db");
            deviceID = rsdata.getHDHRDeviceIDs();
            badSignalCounter = 0;
            currentAzimuth = 0;
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
            //reset bad signal counter
            if (symbolQuality.Equals("100"))
            {
                badSignalCounter = 0;
                return false;
            }
            //check signal Sensitivity
            if (badSignalCounter <= rsdata.singalSensitivity)
            {
                badSignalCounter++;
                Debug.Write("Bad Signal Counter: " + badSignalCounter);
                return false;
            }
            badSignalCounter = 0;
            var channelinfo = channel.Split(':');
            //parse channel, if set by frequency
            if (! channelinfo[0].Equals("auto"))
            {
                if (channelinfo[0].Equals("auto6t"))
                {
                    channelinfo[1] = rsdata.map8vsb(channelinfo[1]);
                }
            }
            channel = channelinfo[1];
            Debug.Write("Channel:" + channel);
            var azimuth = rsdata.getChannelAzimuth(channel);
            //check if antenna is already rotated to the indicated azimuth.
            if (currentAzimuth == azimuth) return false;
            currentAzimuth = azimuth;
            return true;
        }
    }
}
