using System;
using System.Reflection;
using System.IO;

namespace YarnTaskMonitor
{
    public enum logType
    {
        Trace,
        Info,
        Debug,
        Success,
        Failure,
        Warning,
        Error
    }

    class cls_Logger
    {
        private static object logLock;
        private static cls_Logger _instance;

        private cls_Logger() { }

        public static cls_Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new cls_Logger();
                    logLock = new object();
                }
                return _instance;
            }
        }

        public void WriteLog(string content, logType type = logType.Info)
        {
            try
            {
                string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                basePath += @"\SystemLog";
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                //DateTime.Now.ToString("yyyy-MM-dd")

                string[] logText = new string[]
                {
                    DateTime.Now.ToString("hh:mm:ss")+": "+type.ToString()+": "+content
                };

                string fileName = DateTime.Now.ToString("yyyy-MM-dd").ToString();

                lock (logLock)
                {
                    File.AppendAllLines(basePath + "\\" + fileName + ".log", logText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void WriteExceptionLog(Exception ex)
        {
            if (ex != null)
            {
                Type exType = ex.GetType();
                string text;
                text = "Exception: " + exType.Name + Environment.NewLine;
                text += "               " + "Message: " + ex.Message + Environment.NewLine;
                text += "               " + "Source: " + ex.Source + Environment.NewLine;
                text += "               " + "StackTrace: " + ex.StackTrace + Environment.NewLine;
                text += ">>>>>>>>>>>>>>>>>>>>>";
                WriteLog(text, logType.Error);
            }
        }
    }

}
