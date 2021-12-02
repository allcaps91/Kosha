using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

namespace ComBase
{
    /// <summary>
    /// Log4Net Wrapping Class
    /// 1.런타임중 로그레벨을 변경할 수 있으며 로그를 남깁니다
    /// 2. log4net.config의 로그레벨을 변경하면 로그레벨이 변경됩니다.
    /// </summary>
    /// <created>Mentorsoft : donghoonkim 2018-12-26,23:54</created>
    public class Log
    {
        private static ILog LOG => LogManager.GetLogger(new StackFrame(2).GetMethod().DeclaringType);

        static Log()
        {
        }

        /// <summary>
        /// 로그 레벨 변경하기
        /// </summary>
        /// <created>Mentorsoft : donghoonkim 2018-12-26,23:54</created>
        /// <param name="level"></param>
        public void ChangeLogLevel(Level level)
        {
            LogManager.GetRepository().Threshold = level;
        }

        /// <summary>
        /// 로그레벨이 디버그라면 True를 반환합니다
        /// </summary>
        /// <created>Mentorsoft : donghoonkim 2018-12-26,23:54</created>
        public static bool IsDebugEnabled()
        {
            return LOG.IsDebugEnabled;
        }

        /// <summary>
        /// 디버깅 하기 위한 정보를 남깁니다 
        /// </summary>
        /// <created>Mentorsoft : donghoonkim 2018-12-26,23:54</created>
        /// <param name="message"></param>
        /// <param name="values"></param>
        /// <example> Log.Debug("안녕하세요 {}, 저는 {} 입니다", "홍길동", "낭랑19세")</example>
        public static void Debug(string message, params object[] values)
        {
            if (LOG.IsDebugEnabled)
            {
                LOG.Logger.Log(typeof(Log), log4net.Core.Level.Debug, GetMessage(message, values), null);
            }
            
        }

        /// <summary>
        /// 시스템, 이벤트 등의 정보를 남깁니다
        /// </summary>
        /// <created>Mentorsoft : donghoonkim 2018-12-26,23:54</created>
        /// <param name="message"></param>
        /// <param name="values"></param>
        /// <example> Log.Info("안녕하세요 {}, 저는 {} 입니다", "홍길동", "낭랑19세")</example>
        public static void Info(string message, params object[] values)
        {
            if (LOG.IsInfoEnabled)
            {
                LOG.Logger.Log(typeof(Log), log4net.Core.Level.Info, GetMessage(message, values), null);
            }

        }

        /// <summary>
        /// 런타임 오류 또는 문제가 발생하였을경우 정보를 남깁니다
        /// </summary>
        /// <created>Mentorsoft : donghoonkim 2018-12-26,23:54</created>
        /// <param name="message"></param>
        /// <param name="values"></param>
        /// <example> Log.Error("안녕하세요 {}, 저는 {} 입니다", "홍길동", "낭랑19세")</example>
        public static void Error(string message, params object[] values)
        {
            if (LOG.IsErrorEnabled)
            {
                LOG.Logger.Log(typeof(Log), log4net.Core.Level.Error, GetMessage(message, values), null);
            }

        }

        /// <summary>
        /// 치명적인 오류나, 시스템이 더이상 작동이 불가능할 경우 남깁니다.
        /// </summary>
        /// <created>Mentorsoft : donghoonkim 2018-12-26,23:54</created>
        /// <param name="message"></param>
        /// <param name="values"></param>
        /// <example> Log.Fatal("안녕하세요 {}, 저는 {} 입니다", "홍길동", "낭랑19세")</example>
        public static void Fatal(string message, params object[] values)
        {
            if (LOG.IsFatalEnabled)
            {
                LOG.Logger.Log(typeof(Log), log4net.Core.Level.Fatal, GetMessage(message, values), null);
            }

        }


        private static string GetMessage(string message, params object[] values)
        {
            if (values.Length > 0)
            {
                int index = 0;
                while (message.IndexOf("{}") > -1)
                {
                    if (message.IndexOf("{}") > -1)
                    {
                        message = message.Insert(message.IndexOf("{}") + 1, index.ToString());
                        index++;
                        if (index >= values.Length)
                        {
                            break;
                        }
                    }
                }
                message = message.Replace("{}", string.Empty);
                return string.Format(message, values);

            }
            else
            {
                return message;
            }
           
        }

        public static void Debug(object message) => LOG.Logger.Log(typeof(Log), log4net.Core.Level.Debug, message, null);

        public static void Info(object message, Exception e) => LOG.Logger.Log(typeof(Log), log4net.Core.Level.Info, message, e);

        public static void Warn(object message) => LOG.Logger.Log(typeof(Log), log4net.Core.Level.Warn, message, null);

        public static void Error(object message) => LOG.Logger.Log(typeof(Log), log4net.Core.Level.Error, message, null);

        public static void Fatal(object message) => LOG.Logger.Log(typeof(Log), log4net.Core.Level.Fatal, message, null);



    }
}
