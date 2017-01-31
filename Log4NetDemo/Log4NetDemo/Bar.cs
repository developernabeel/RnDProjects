using System;
using System.Threading;
using log4net;

namespace Log4NetDemo
{
    public class Bar
    {
        private static readonly ILog BarLogger = LogManager.GetLogger("BarLogger");

        public Bar()
        {
            BarLogger.Info("Bar log");
        }
    }
}
