using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite; //SQLite-net PCL - https://github.com/praeclarum/sqlite-net
using System.IO;

namespace RotorSync
{
    public class RSData : SQLiteDb //from DataAccess.cs
    {
        //properties
        private SQLiteConnection mydb;
        private HDHR HDHRTable;

        //public RSConfig properties
        public string HDHRDeviceID
        {
            get
            {
                return mydb.Find<config>(1).HDHRdeviceID;
            }
        }
        public long singalSensitivity {
            get
            {
                return mydb.Find<config>(1).sensitivity;
            }
        }
        public long? rotorTimeout {
            get {
                return mydb.Find<config>(1).rotorTimeout;
            }
        }

        public long? rotorAzimuth
        {
            get
            {
                return mydb.Find<config>(1).rotorAzimuth;
            }
            set
            {
                var configRow = mydb.Find<config>(1);
                configRow.rotorAzimuth = value;
                int rowsAffected = mydb.Update(configRow);
                
            }
        }
        public string rotorCOMPort
        {
            get
            {
                return mydb.Find<config>(1).rotorCOMPort;
            }
        }
        public long? homeChannel
        {
            get
            {
                return mydb.Find<config>(1).homeChannel;
            }
        }
        public long? homeChannelTimeout
        {
            get
            {
                return mydb.Find<config>(1).homeChannelTimeout;
            }
        }

        //constructors
        public RSData(string path) : base(path)
        {
            //get this string from the registry?
            this.Create();
            mydb = new SQLiteConnection(path);
            //count config rows
            if (mydb.Table<config>().Count() != 1)
            {
                //load initial data
                loadSQL(Properties.Resources.RSData.ToString());
            }
        }

        private void loadSQL(string sqldata)
        {
            //we have to break appart the sql because execute only executes one command!!
            //http://www.damirscorner.com/blog/posts/20140422-SqliteOnlyExecutesTheFirstStatementInACommand.html

            var statements = sqldata.Split(new[] { ';' },
            StringSplitOptions.RemoveEmptyEntries);
            foreach (var statement in statements)
            {
                mydb.Execute(statement);
            }
        }

        /*
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
        */

        public string map8vsb(string freqstr)
        {
            //clear traling 0
            freqstr = freqstr.Trim('0');
            var result = mydb.Table<frequencymap>().Where(v => v.frequency.Equals(freqstr));
            foreach (var map in result) return map.channel.ToString();
            return "0";
        }

        //return azimuth for channel, or negativa value if none found
        public Int64 getChannelAzimuth(string channel)
        {
            var result = mydb.Table<channels>().Where(v => v.realchannel.Equals(channel));
            foreach (var map in result) return map.azimuth;
            return -1;
        }
    }
}
