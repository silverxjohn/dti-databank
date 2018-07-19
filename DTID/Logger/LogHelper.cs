using DTID.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DTID.Logger
{
    public class LogHelper
    {
        private List<ILogger> Logger;

        public LogHelper(ApplicationDbContext context, int userId)
        {
            Logger = new List<ILogger>{
                new FileLogger(),
                new DatabaseLogger(context, userId)
            };
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
            foreach (var logger in Logger) {
                logger.Log(message);
            }
        }
    }
}
