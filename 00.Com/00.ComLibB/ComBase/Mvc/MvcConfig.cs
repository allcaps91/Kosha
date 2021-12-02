using ComBase.Mvc.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc
{
    public class MvcConfig
    {
        public void SetLog4Net()
        {
            try
            {

                string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4Net.config");
                FileInfo fileInfo = new FileInfo(logFilePath);
                log4net.Config.XmlConfigurator.ConfigureAndWatch(fileInfo);
                Log.Info("==============================================================");
                Log.Info("=        Log4net 설정 Assemblyinfo.cs, log4net.config       = ");
                Log.Info("=        Log4net 로그설정파일 PATH: {}                           = ", logFilePath);
                Log.Info("=        Log4net LogFullpath: {}                           = ", logFilePath);
                Log.Info("==============================================================");


            }
            catch (FileNotFoundException ex)
            {
                throw new MTSException("og4Net.config 파일을 찾을 수 없습니다.", ex);
            }
            catch (Exception ex)
            {
                throw new MTSException("로그 설정중 오류가 발생하였습니다", ex);
            }
        }
    }
}
