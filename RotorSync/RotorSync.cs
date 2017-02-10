using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private Log log;
        public RotorSync()
        {
            InitializeComponent();

            rsdata = new RSData(Properties.Settings.Default.DBPath);
            log = new Log();
        }

        protected override void OnStart(string[] args)
        {
            //RSInstance = new RS("10178456");
            //RSInstance = new RS("1050A596");
            string HDHRDeviceID = rsdata.HDHRDeviceID;
            if (HDHRDeviceID == "")
            {
                log.log("No HDHR Device ID set.  exiting");
                this.Stop();
            }
            log.log("Setting DeviceID to " + HDHRDeviceID);
            try
            {
                RSInstance = new RS(HDHRDeviceID);
            } catch (System.IO.IOException e)
            {
                log.log("Cannot open rotor com port, exiting\n" + e.Message);
                this.Stop();
            } catch (UnauthorizedAccessException)
            {
                log.log("Rotor com port in use, exiting");
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
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            if (RSInstance.DoSync())
            {
                log.log("\nRotating to " + RSInstance.currentAzimuth + " for channel " + RSInstance.channel);
            }
        }
    }
}
