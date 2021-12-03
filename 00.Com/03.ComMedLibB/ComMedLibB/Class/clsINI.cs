using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ComMedLibB
{
    /// <summary>
    /// 2018.01.05 이상훈 Create.  ENT 후두경 장비 Interface 위한 INI File 생성 목적
    /// </summary>
    public class clsINI
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
