using ComBase;
using minidump;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComBase
{
    public class SystemConfiguration
    {
      
        public static void GlobalExceipnt()
        {
            //처리되지 않은 예외는 Application_ThreadException 핸들러 발생하고 프로그램은 유지됨
           Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
           Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

           string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4Net.config");
           FileInfo fileInfo = new FileInfo(logFilePath);
           log4net.Config.XmlConfigurator.ConfigureAndWatch(fileInfo);

           Log.Info("==========={}  GLobal UnhandledException 이벤트 및 로그설정 =========== " , AppDomain.CurrentDomain.FriendlyName);
           try
           {         
               String folderName = "C:\\HealthSoft\\LOG";
               DirectoryInfo di = new DirectoryInfo(folderName);
               DateTime today = DateTime.Now;
               foreach (FileInfo file in di.GetFiles())
               {
                    if (file.Extension.ToLower().Equals(".log") || file.Extension.ToLower().Equals(".txt") || file.Extension.ToLower().Equals(".jpg") || file.Extension.ToLower().Equals(".dmp"))
                    {
                        if (file.LastWriteTime < today.AddDays(-7))
                        {                             
                             Log.Info("file delete: {}", file.Name);
                             file.Delete();
                        }
                    }                   
               }   
           }catch(Exception e)
           {
               Log.Error(e.ToString());
           }
        }
      
       
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Log.Error(e.Exception.ToString());
            Log.Error(clsPublic.GstrSabunName + clsPublic.GstrSabun + clsPublic.GstrPtno, clsPublic.gName, clsPublic.GstrBuseName);
            MiniDump.CreateMiniDump();

            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                string fullpath = "C:\\HealthSoft\\LOG\\screenshot_" + DateTime.Now.ToString("yyyy-MM-dd.HHmmss") +".jpg";

                bitmap.Save(fullpath, ImageFormat.Jpeg);
            }
                              
            MessageBox.Show(new Form() { TopMost = true }, "Global 오류가 발생하였습니다 ");
        }
    }
}
