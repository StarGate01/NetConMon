using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetConMon
{

    public class SharedValues
    {

        public const string SVC_NAME = "NetConMon";

        public const string REG_KEY = @"SYSTEM\CurrentControlSet\Services\NetConMon\Parameters";
        public const string REG_PARAM_PING_IP = "IP";
        public const string REG_PARAM_PING_TIMEOUT = "Timeout";
        public const string REG_PARAM_PING_INTERVAL = "Interval";

        public const string EV_SOURCE = "NetMonCon Status Change";
        public const string EV_LOG = "NetMonConLog";
        public const int EV_ID_TIMEOUT = 1;
        public const int EV_ID_SUCCESS = 0;
        public const int EV_ID_STARTSTOP = 3;

        public const string DEFAULT_PING_IP = "8.8.8.8";
        public const int DEFAULT_PING_TIMEOUT = 500;
        public const int DEFAULT_PING_INTERVAL = 5000;

    }

}
