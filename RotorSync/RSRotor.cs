using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;

namespace RotorSync
{
    class RSRotor
    {
        public string COMPort;
        public string currentAzimuth {
            get
            {
                com.Write("C");
                com.Write("\r\n");
                string result = com.ReadLine();
                var status = result.Split(new[] { ' ' })
                .Select(part => part.Split('='))
                .ToDictionary(split => split[0], split => split[1]);
                return status["AZ"];
            } }
        private RSData rsdata;
        private SerialPort com;

        public RSRotor()
        {
            rsdata = new RSData(Properties.Settings.Default.DBPath);
            COMPort = rsdata.rotorCOMPort;
            com = new SerialPort(COMPort, 9600);
            /*
            com = new SerialPort();
            com.PortName = COMPort;
            com.BaudRate = 9600;
            com.DataBits = 8;
            com.StopBits = StopBits.One;
            com.Parity = Parity.None;
            com.Handshake = Handshake.XOnXOff;
            */

            //Debug.WriteLine("Opening: " + COMPort);
            com.Open();
        }

        public void RotateToAzimuth(string azimuth)
        {
            string tosend = "S\r\nM" + azimuth.PadLeft(3, '0');
            //Debug.WriteLine("Sending " + tosend + " to " + COMPort);
            com.Write(tosend);
            com.Write("\r\n");
        }

    }
}
