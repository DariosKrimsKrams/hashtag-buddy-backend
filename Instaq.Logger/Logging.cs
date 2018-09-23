namespace Instaq.Logger
{
    using System;
    using System.IO;

    public class Logging
    {
        private const string filename = "C:/Instaq/log.txt";

        public static void DumpLog()
        {
            using (StreamReader r = File.OpenText(filename))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        public static void Log(string logMessage)
        {
            using (StreamWriter w = File.AppendText(filename))
            {
                w.WriteLine("{0}", logMessage);
            }
        }

        public static void LogInline(string logMessage)
        {
            using (StreamWriter w = File.AppendText(filename))
            {
                w.Write("{0}", logMessage);
            }
        }
    }
}
