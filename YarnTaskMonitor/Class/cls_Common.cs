using System;

namespace YarnTaskMonitor
{
    class cls_Common
    {
        //config
        public static string connetionCmd;
        public static string table;
        public static string autoInterval;
        public static string taskBar;

        //hwnd message
        public static IntPtr hwndFrmMain;
        public const int USER = 0x0400;
        public const int AUTO_LOCK_UI = USER + 1;
        public const int AUTO_RELEASE_UI = AUTO_LOCK_UI + 1;
    }
}
