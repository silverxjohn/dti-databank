using DTID.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DTID.Logger
{
    public class FileLogger : ILogger
    {
        private const string PATH = "wwwroot/logs/dti.log";

        public FileLogger()
        {
        }

        public void Log(string message)
        {
            var time = DateTime.Now.ToString("G") + ": ";

            File.AppendAllText(PATH, time + message + Environment.NewLine);
        }
    }
}
