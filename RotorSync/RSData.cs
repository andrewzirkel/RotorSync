using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace RotorSync
{
    public class RSData : SQLiteDb
    {
        //properties
        private SQLiteConnection db;
        private HDHR HDHRTable;

        //public RSConfig properties
        public string HDHRDeviceID
        {
            get
            {
                return db.Find<config>(1).HDHRdeviceID;
            }
        }
        public long singalSensitivity {
            get
            {
                return db.Find<config>(1).sensitivity;
            }
        }
        public long? rotorTimeout {
            get {
                return db.Find<config>(1).rotorTimeout;
            }
        }

        public long? rotorAzimuth { get { return db.Find<config>(1).rotorAzimuth; } }
        public string rotorCOMPort
        {
            get
            {
                return db.Find<config>(1).rotorCOMPort;
            }
        }

        //constructors
        public RSData(string path) : base(path)
        {
            //get this string from the registry?
            db = new SQLiteConnection(path);
        }

        public string getHDHRDeviceIDs()
        {
            string id = null;
            var query = db.Table<HDHR>();
            int i = 0;
            foreach (var row in query)
            {
                id = row.deviceID;
            }
            return id;
        }

        public string map8vsb(string freqstr)
        {
            //clear traling 0
            freqstr = freqstr.Trim('0');
            var result = db.Table<frequencymap>().Where(v => v.frequency.Equals(freqstr));
            foreach (var map in result) return map.channel.ToString();
            return "0";
        }

        public Int64 getChannelAzimuth(string channel)
        {
            var result = db.Table<channels>().Where(v => v.realchannel.Equals(channel));
            foreach (var map in result) return map.azimuth;
            return 0;
        }
    }
}
