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
        private RSRotor rotor;

        //keeping track of timeouts
        private int badSignalCounter;
        private int homeChannelTimeoutCounter;

        //passing messages back up the chain
        public bool messageLight { get; private set; }
        public string message { get; private set; }

        public string channel { get; private set; }
        public string signalStrength { get; private set; }
        public string signalQuality { get; private set; }
        public string symbolQuality { get; private set; }
        public long? currentAzimuth
        {
            get
            {
                return rsdata.rotorAzimuth;
            }
            private set
            {
                rsdata.rotorAzimuth = value;
            }

        }
        private long homeAzimuth;

        //constructor
        public RS(string id)
        {
            deviceID = id;
            hdhr = new RSHDHomeRunDevice(deviceID);
            rsdata = new RSData(Properties.Settings.Default.DBPath);
            rotor = new RSRotor();
            //deviceID = rsdata.getHDHRDeviceIDs();
            badSignalCounter = 0;
            homeChannelTimeoutCounter = 0;
            currentAzimuth = 0;
            homeAzimuth = rsdata.getChannelAzimuth(rsdata.homeChannel.ToString());
        }

        public bool DoSync()
        {
            List<int> activeTuners = hdhr.activeTuners;
            if ( activeTuners.Count == 0)
            {
                if ((homeChannelTimeoutCounter++ > rsdata.homeChannelTimeout) && (currentAzimuth != homeAzimuth))
                {
                    //Debug.Write("Current azimuth: " + currentAzimuth + " Rotate to: " + homeAzimuth + "\n");
                    currentAzimuth = homeAzimuth;
                    rotor.RotateToAzimuth(homeAzimuth.ToString());
                    channel = "Default";
                    return true;
                }
                return false;
            }
            if (activeTuners.Count == 1)
            {
                string tunerNum = activeTuners[0].ToString();

                channel = hdhr.RSHDHomeRunGet("/tuner" + tunerNum + "/channel");
                homeChannelTimeoutCounter = 0;
                string statusRaw = hdhr.RSHDHomeRunGet("/tuner" + tunerNum + "/status");
                var status = statusRaw.Split(new[] { ' ' })
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
                if (badSignalCounter < rsdata.singalSensitivity)
                {
                    badSignalCounter++;
                    //Debug.Write("Bad Signal Counter: " + badSignalCounter + "\n");
                    return false;
                }
                badSignalCounter = 0;
                var channelinfo = channel.Split(':');
                //parse channel, if set by frequency
                if (!channelinfo[0].Equals("auto"))
                {
                    if (channelinfo[0].Equals("auto6t") || channelinfo[0].Equals("8vsb"))
                    {
                        channelinfo[1] = rsdata.map8vsb(channelinfo[1]);
                    }
                }
                channel = channelinfo[1];
                var azimuth = rsdata.getChannelAzimuth(channel);
                //check for valid azimuth
                if (azimuth < 0) return false;
                //check if antenna is already rotated to the indicated azimuth.
                //Debug.Write("Channel:" + channel + " azimuth: " + azimuth + "\n");
                if (currentAzimuth == azimuth) return false;
                //Debug.Write("Current azimuth: " + currentAzimuth + " Rotate to: " + azimuth + "\n");
                currentAzimuth = azimuth;
                rotor.RotateToAzimuth(azimuth.ToString());
                return true;
            }
            //if we get here then more than 1 channel is tuned and we do nothing
            return false;
        }
    }
}
