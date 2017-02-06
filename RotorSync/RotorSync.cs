using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RotorSync
{
    public partial class RotorSync : ServiceBase
    {
        private RS RSInstance;
        private RSData rsdata;
        public RotorSync()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("RotorSync"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "RotorSync", "");
            }
            eventLog1.Source = "RotorSync";
            eventLog1.Log = "";

            rsdata = new RSData(Properties.Settings.Default.DBPath);
        }

        protected override void OnStart(string[] args)
        {
            //RSInstance = new RS("10178456");
            //RSInstance = new RS("1050A596");
            string HDHRDeviceID = rsdata.HDHRDeviceID;
            if (HDHRDeviceID == "")
            {
                eventLog1.WriteEntry("No HDHR Device ID set.  exiting");
                this.Stop();
            }
            eventLog1.WriteEntry("In OnStart.  " + "Config Data: " + "DeviceID: " + HDHRDeviceID);
            try
            {
                RSInstance = new RS(HDHRDeviceID);
            } catch (System.IO.IOException e)
            {
                eventLog1.WriteEntry("Cannot open rotor com port, exiting\n" + e.Message,EventLogEntryType.Error);
                this.Stop();
            } catch (UnauthorizedAccessException)
            {
                eventLog1.WriteEntry("Rotor com port in use, exiting", EventLogEntryType.Error);
                this.Stop();
            }
            
            // Set up a timer to trigger every minute.
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000; // 1 seconds
            //timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In onStop.");
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            //eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
            //eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information);
            /*
            try
            {
                bool resuult = RSInstance.DoSync();
            } catch (Exception ex)
            {

                throw ex;
            }
            */
            if (RSInstance.DoSync())
                eventLog1.WriteEntry("Channel: " + RSInstance.channel + 
                    "\nSymbol Quality: " + RSInstance.symbolQuality +
                    "\nRotating to: " + RSInstance.currentAzimuth, EventLogEntryType.Information);
        }
    }
}
