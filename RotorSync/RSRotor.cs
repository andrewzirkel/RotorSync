using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace RotorSync
{
    class RSRotor
    {
        public string COMPort;
        private RSData rsdata;
        private SerialPort com;

        public RSRotor()
        {
            rsdata = new RSData("C:\\Code\\RotorSync\\RotorSync\\RSDatabase.db");
            COMPort = rsdata.rotorCOMPort;
            com = new SerialPort(COMPort);
            com.Open();
        }

        public void RotateToAzimuth(string azimuth)
        {
            com.WriteLine("M" + azimuth);
        }
    }
}
