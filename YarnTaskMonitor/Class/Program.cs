using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace YarnTaskMonitor
{
    static class Program
    {

        #region API
        //禁止多个进程运行,并当重复运行时激活以前的进程
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int WS_SHOWNORMAL = 1; //正常弹出窗体
        //private const int WS_SHOWNOACTIVATE = 4; //激活窗体，恢复窗体，还原窗体
        //private const int WS_SHOW = 5; //显示窗体，最小化时不会最大化
        #endregion

        #region Application Enterance
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Get the running instance.
            Process instance = RunningInstance();
            if (instance == null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frm_Main());
            }
            else
                //There is another instance of this process
                HandleRunningInstance(instance);
        }
        #endregion

        #region Function
        /// <summary>
        /// 获取进程实例是否已启动
        /// </summary>
        /// <returns></returns>
        private static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //Loop through the running processes in with the same name   
            foreach (Process process in processes)
            {
                //Ignore the current process   
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file.   
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //Return   the   other   process   instance.   
                        return process;
                    }
                }
            }
            //No other instance was found, return null. 
            return null;
        }

        /// <summary>
        /// 已有实例启动时激活实例
        /// </summary>
        /// <param name="instance"></param>
        private static void HandleRunningInstance(Process instance)
        {
            //Make sure the window is not minimized or maximized   
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL);
            //Set the real intance to foreground window
            SetForegroundWindow(instance.MainWindowHandle);
        }
        #endregion

    }
}
