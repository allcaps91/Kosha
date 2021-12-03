using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ComHpcLibB
{
    public class INIFile
    {
        //FUN.IniWriteValue("BROWSER", "SIZE", trbSize.Value.ToString(), clsVariable.GetApplicationPath().ToString());

        //INI Read Write를 위한 API 선언
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(    // INI Read
            String section,
            String key,
            String def,
            StringBuilder retVal,
            int size,
            String filePath);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(  // INI Write
            String section,
            String key,
            String val,
            String filePath);

        //INI File Read
        public String IniReadValue(String Section, String Key, String iniPath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
            return temp.ToString();
        }

        //INI File Write
        public void IniWriteValue(String Section, String Key, String Value, String iniPath)
        {
            WritePrivateProfileString(Section, Key, Value, iniPath);
        }
    }
}
