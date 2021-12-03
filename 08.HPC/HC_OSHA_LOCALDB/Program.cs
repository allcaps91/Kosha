using ComBase;
using ComBase.Mvc;
using ComBase.Mvc.Utils;
using HC_Core.Service;
using System;
using System.Windows.Forms;

namespace HC_OSHA_LOCALDB
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new MvcConfig().SetLog4Net();

            //      clsDB.DbCon = clsDB.DBConnect("192.168.100.31", "1521", "ora7", "KOSMOS_PMPA", "hospital");

            //clsDB.DbCon = clsDB.DBConnect("localhost", "1521", "XE", "KOSMOS_PMPA", "hospital");
            clsDB.DbCon = clsDB.DBConnect("localhost", "1521", "ORCL", "KOSMOS_PMPA", "hospital");
            if (clsDB.DbCon == null)
            {
                MessageUtil.Alert("Local Database에 접속할 수 없습니다");
                Application.Exit();
            }
            else
            {
                DataSyncService.Instance.IsLocalDB = true;

                Application.Run(new Login());
            }

        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //오류정보 전송 팝업, 클라이언트 아이피 표시, 프로그램에도 버전 및 아이피 표시
            Logging(e.Exception);
            MessageUtil.Error("전역오류: " + e.Exception.StackTrace);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public static void Logging(Exception e)
        {
            Log.Fatal("Global Exception Full Stacktrace: \r\n" + e.ToString());
        }
    }
}
