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
        //properties
        private string deviceID;
        private HDHomeRun hdhr;

        //constructor
        public RSHDHomeRunDevice() {
            if (deviceID != null) { hdhr = new HDHomeRun(deviceID); }
        }

        //constructor with device id
        public RSHDHomeRunDevice(string id)
        {
            deviceID = id;
            hdhr = new HDHomeRun(deviceID);
        }

        public string RSHDHomeRunGet(string var)
        {
            return hdhr.GetVariable(var);
        }
    }
}
