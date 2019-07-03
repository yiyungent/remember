using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CornJob
{
    public class LogHelper
    {
        private readonly static string _logPath = ConfigurationManager.AppSettings["LogPath"];

        public static void Log(string message)
        {
            File.AppendAllText(_logPath, Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine + message + Environment.NewLine);
        }
    }
}
