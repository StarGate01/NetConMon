using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Net.NetworkInformation;
using Microsoft.Win32;

namespace NetConMon
{
    public partial class NetConMon : ServiceBase
    {

        private string ping_ip = SharedValues.DEFAULT_PING_IP;
        private int ping_timeout = SharedValues.DEFAULT_PING_TIMEOUT;
        private int ping_interval = SharedValues.DEFAULT_PING_INTERVAL;

        private Timer timer;
        private Ping ping;
        private PingReply pingReply;

        private bool wasWorking = true;
        private bool firstPing = true;

        public NetConMon()
        {
            InitializeComponent();

            eventLog1 = new EventLog();
            if (!EventLog.SourceExists(SharedValues.EV_SOURCE)) EventLog.CreateEventSource(SharedValues.EV_SOURCE, SharedValues.EV_LOG);
            eventLog1.Source = SharedValues.EV_SOURCE;
            eventLog1.Log = SharedValues.EV_LOG;

            timer = new Timer();
            timer.Elapsed += Timer_Elapsed;

            ping = new Ping();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            pingReply = ping.Send(ping_ip, ping_timeout);
            if(pingReply.Status != IPStatus.Success)
            {
                if(wasWorking || firstPing)
                {
                    eventLog1.WriteEntry("Network timed out", 
                        EventLogEntryType.Warning, SharedValues.EV_ID_TIMEOUT);
                    wasWorking = false;
                }
            }
            else
            {
                if (!wasWorking)
                {
                    eventLog1.WriteEntry("Network is reachable", 
                        EventLogEntryType.Information, SharedValues.EV_ID_SUCCESS);
                    wasWorking = true;
                }
            }
            firstPing = false;
        }

        protected override void OnStart(string[] args)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(SharedValues.REG_KEY))
            {
                ping_ip = (string)key.GetValue(SharedValues.REG_PARAM_PING_IP);
                ping_timeout = (int)key.GetValue(SharedValues.REG_PARAM_PING_TIMEOUT);
                ping_interval = (int)key.GetValue(SharedValues.REG_PARAM_PING_INTERVAL);
            }

            if (args.Count() > 0)
            {
                ping_ip = args[0];
                if (args.Count() > 1) int.TryParse(args[1], out ping_timeout);
            }

            eventLog1.WriteEntry("Network monitor started" + Environment.NewLine + 
                "Target IP adress: " + ping_ip + Environment.NewLine + 
                "Ping timeout: " + ping_timeout + " ms" + Environment.NewLine +
                "Ping interval: " + ping_interval + " ms",
                EventLogEntryType.Information, SharedValues.EV_ID_STARTSTOP);

            timer.Interval = ping_interval;
            timer.Start();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Network monitor stopped", 
                EventLogEntryType.Information, SharedValues.EV_ID_STARTSTOP);
        }

    }

}
