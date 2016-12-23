﻿using System;
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
        }

        protected override void OnStart(string[] args)
        {
            //RSInstance = new RS("10178456");
            RSInstance = new RS("1050A596");
            eventLog1.WriteEntry("In OnStart");
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
            if (RSInstance.DoSync())
                eventLog1.WriteEntry("Channel: " + RSInstance.channel + " Symbol Quality: " + RSInstance.symbolQuality, EventLogEntryType.Information);
        }
    }
}
