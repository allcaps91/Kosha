using ComBase;
using ComBase.Mvc;
using ComBase.Mvc.Utils;
using HC_Measurement;
using HC_OSHA;
using System;
using System.Windows.Forms;

namespace HC_LAUNCHER
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

            //     clsType.User.Sabun = "800594";//한흥렬
            //    clsType.User.UserName = "한흥렬";//한흥렬

            //     clsType.User.Sabun = "36513";//한흥렬
            //    clsType.User.UserName = "이경미";//한흥렬

            //                              //clsType.User.Sabun = "800594"; // 테스트 계정 모든 권한
            //                              //clsType.User.UserName = "테스트"; // 테스트 계정 모든 권한

            //    clsType.User.Sabun = "33398"; // 김중구
            //     clsType.User.UserName = "김중구"; // 김중구

            //  clsType.User.Sabun = "41783"; // 
            //   clsType.User.UserName = "강은숙"; // 
              clsType.User.Sabun = "800594"; // 
              clsType.User.UserName = "한흥렬"; // 
            clsDB.DbCon = clsDB.DBConnect("192.168.100.31", "1521", "ORA7", "KOSMOS_PMPA", "hospital");
            //DataSyncService.Instance.IsLocalDB = false;

            //  clsDB.DbCon = clsDB.DBConnect("192.168.100.35", "1525", "PSMHTEST", "KOSMOS_ERP", "hospital");
            // DataSyncService.Instance.IsLocalDB = true;


            //Application.Run(new Dashboard());
            Application.Run(new frmMDI_WE());
         //   Application.Run(new Login());
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
