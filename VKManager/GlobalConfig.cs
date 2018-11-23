using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKManager
{
    public static class GlobalConfig
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public static string AccessToken = default;
        public static int UserID = default;
    }
}
