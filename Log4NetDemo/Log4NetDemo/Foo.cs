using System;
using System.Threading;
using log4net;
using System.IO;

namespace Log4NetDemo
{
    // Code sample taken from :
    // http://www.codeproject.com/Tips/1107824/Perfect-Log-Net-with-Csharp

    class Foo
    {
        private static readonly ILog FooLogger = LogManager.GetLogger("FooLogger");

        public Foo()
        {
            // Specify the file from where to read Log4Net configuration data.
            // If used in web application, add this line to Application_Start of gloabal.asax.cs file.
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("Log4Net.config"));
        }

        static void Main(string[] args)
        {
            new Foo();
            try
            {
                LogManager.GetLogger(typeof(Foo)).Fatal("General Log");

                LogAllTypes();
                new Bar();

                int a = 0;
                int b = 1 / a;
            }
            catch (Exception ex)
            {
                FooLogger.Error(ex.ToString());
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void LogAllTypes()
        {
            double secs = 0.1;
            FooLogger.Fatal("Start log FATAL...");
            Console.WriteLine("Start log FATAL...");
            Thread.Sleep(TimeSpan.FromSeconds(secs));

            FooLogger.Error("Start log ERROR...");
            Console.WriteLine("Start log ERROR...");
            Thread.Sleep(TimeSpan.FromSeconds(secs)); 

            FooLogger.Warn("Start log WARN...");
            Console.WriteLine("Start log WARN...");
            Thread.Sleep(TimeSpan.FromSeconds(secs)); 

            FooLogger.Info("Start log INFO...");
            Console.WriteLine("Start log INFO...");
            Thread.Sleep(TimeSpan.FromSeconds(secs)); 

            FooLogger.Debug("Start log DEBUG...");
            Console.WriteLine("Start log DEBUG...");
            Thread.Sleep(TimeSpan.FromSeconds(secs));

        }
    }
}
