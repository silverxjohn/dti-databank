using DTID.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DTID.Logger
{
    public class LogHelper
    {
        private List<ILogger> FileLogger;
        private List<ILogger> DatabaseLogger;

        public LogHelper(ApplicationDbContext context)
        {
            FileLogger = new List<ILogger>{
                new FileLogger(),
                new DatabaseLogger(context)
            };

            //DatabaseLogger = new List<ILogger>
            //{
                
            //};
        }

        public void Log(Action action, object model, [CallerFilePath] string module = null)
        {
            var fileName = module.Split("\\");
            var controllerName = fileName[fileName.Length - 1].Split(".")[0];
            var modelName = controllerName.Substring(0, controllerName.Length - 11);

            var message = $"{modelName} {model.ToString()} has been ";

            switch (action)
            {
                case Action.Create:
                    message += "created";
                    break;
                case Action.Update:
                    message += "updated";
                    break;
                case Action.Delete:
                    message += "deleted";
                    break;
                case Action.Review:
                    message += "reviewed";
                    break;
            }

            WriteLog(message);
        }

        private void WriteLog(string message)
        {
            foreach (var logger in FileLogger) {
                logger.Log(message);
            }
        }
    }
}
