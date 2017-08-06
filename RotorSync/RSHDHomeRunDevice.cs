using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDHomeRunWrapper;

namespace RotorSync
{
    class RSHDHomeRunDevice
    {
        //private properties
        private string deviceID;
        private HDHomeRun hdhr;
        private int activeTuner = 0;

        //public properties
        private int tunerCount = 0;
        public List<int> activeTuners
        {
            get
            {
                string channel;
                List<int> tuners = new List<int>();
                for (int i = 0; i < tunerCount; i++)
                {
                    channel = this.RSHDHomeRunGet("/tuner" + i + "/channel");
                    if (! channel.Equals("none"))
                    {
                         tuners.Add(i);
                    }
                }
                return tuners;
            }
        }


        //constructor
        public RSHDHomeRunDevice() {
            if (deviceID != null) { hdhr = new HDHomeRun(deviceID); }
        }

        //constructor with device id
        public RSHDHomeRunDevice(string id)
        {
            deviceID = id;
            hdhr = new HDHomeRun(deviceID);
            setTunerCount();
        }

        //Private methods

        private void setTunerCount()
        {
            tunerCount = 0;
            int i = 0;
            string var = "";
            while(true)
            {
                try
                {
                    var = "/tuner" + i + "/status";
                    hdhr.GetVariable(var);
                } catch (System.Exception e) {
                    if (tunerCount == 0)
                    {
                        throw e;
                    }
                    break;
                }
                i++;
                tunerCount = i;
            }

        }

        //public methods

        public string RSHDHomeRunGet(string var)
        {
            return hdhr.GetVariable(var);
        }
    }
}
