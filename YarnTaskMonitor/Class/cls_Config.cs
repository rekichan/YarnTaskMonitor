using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YarnTaskMonitor
{
    class cls_Config
    {

        #region Properties
        public static cls_Config config;
        private object lockobject;//多线程被锁对象
        private string iniPath;
        #endregion

        #region API
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        #endregion

        #region Constructor
        private cls_Config(string path) {
            iniPath = path;
            lockobject = new object();
        }
        #endregion

        #region Function
        /// <summary>
        /// 获取config实例
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <returns></returns>
        public static cls_Config getInstance(string path = "c:\\config.ini" )
        {
            if (config != null)
                return config;

            if (!System.IO.File.Exists(path))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                System.IO.FileStream fs = System.IO.File.Create(path);
                fs.Close();
            }

            if (config == null)
                config = new cls_Config(path);

            return config;
        }

        /// <summary>
        /// 写入INI文件 
        /// </summary>
        /// <param name="Section">项目名称(如 [TypeName])</param>
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            lock (lockobject)
            {
                WritePrivateProfileString(Section, Key, Value, this.iniPath);
            }
        }

        /// <summary>
        /// 读出INI文件 
        /// </summary>
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key, string sDefault)
        {
            lock (lockobject)
            {
                StringBuilder temp = new StringBuilder(500);
                int i = GetPrivateProfileString(Section, Key, sDefault, temp, 500, this.iniPath);
                if (temp.Length == 0)
                {
                    temp.Append(sDefault);
                }
                return temp.ToString();
            }
        }
        #endregion

    }
}
