using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Growl.Connector;
using System.Diagnostics;

namespace RotorSync
{
    class Log
    {
        //growl private variables
        private GrowlConnector growl;
        private NotificationType notificationType;
        private Growl.Connector.Application application;
        private string sampleNotificationType = "SAMPLE_NOTIFICATION";

        //eventlog private
        private System.Diagnostics.EventLog eventlog;

        public Log()
        {
            //setup growl
            notificationType = new NotificationType(sampleNotificationType, "Sample Notification");
            growl = new GrowlConnector();
            application = new Growl.Connector.Application("RotorSync");
            growl.Register(application, new NotificationType[] { notificationType });

            //setup event log
            eventlog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("RotorSync"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "RotorSync", "");
            }
            eventlog.Source = "RotorSync";
            eventlog.Log = "";
        }

        public void log(string message)
        {
            logToEventLog(message);
            logToGrowl(message);
        }

        private void logToEventLog(string message)
        {
            eventlog.WriteEntry(message);
        }

        private void logToGrowl(string message)
        {
            Notification notification = new Notification(application.Name, this.notificationType.Name, DateTime.Now.Ticks.ToString(), "RotorSync", message);
            this.growl.Notify(notification);
        }
    }
}
