using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotorSync
{
    class RSTuner
    {
        //properties
        private string deviceID;
        private int tuner;
        //default constructor
        RSTuner() { }

        //constructor with options
        RSTuner(string devid, int tuner)
        {
            this.deviceID = devid;
            this.tuner = tuner;
        }
    }
}
