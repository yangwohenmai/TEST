using log4net;
using log4net.Repository;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

/******************************************************************
 * Author: miaoxin 
 * Date: 2018-10-23
 * Content: Log4Net日志类 
 * 实例方法：Log4NetUtil.Error(this, "msg"); 静态方法：Log4NetUtil.Error(typeof(Program), "msg");
 ******************************************************************/

namespace CommonLib.DbAccess
{
    /// <summary>
    /// Log4Net日志类
    /// </summary>
    public class Log4NetUtil
    {
        private static string repositoryName = "Log4NetRepository";
        private static ILoggerRepository repository = null;
        private static readonly ConcurrentDictionary<Type, ILog> dic_Loggers = new ConcurrentDictionary<Type, ILog>();

        #region 初始化日志设置
        /// <summary>
        /// 初始化日志设置
        /// </summary>
        /// <param name="AppBasePath">应用根路径(绝对路径)</param>
        public static void InitLogSetting(string AppBasePath)
        {
            if (string.IsNullOrEmpty(AppBasePath))
            {
                throw new ArgumentException("Log4NetUtil.InitLogSetting->AppBasePath is Empty");
            }
            string strConfPath = Path.Combine(AppBasePath, "config", "log4net.config");
            if (!File.Exists(strConfPath))
            {
                throw new Exception("Log4NetUtil.InitLogSetting->" + strConfPath + " Not Exists");
            }
            repository = LogManager.CreateRepository(repositoryName);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(repository, new FileInfo(strConfPath));
        }
        #endregion

        #region 取得日志路径
        /// <summary>
        /// 取得日志路径(当日日志路径)
        /// </summary>
        /// <param name="appenderName">AppenderName</param>
        /// <returns></returns>
        public static string GetLogPath(string appenderName)
        {
            //日志路径
            string logPath = string.Empty;

            if (string.IsNullOrEmpty(appenderName) || repository == null)
            {
                return logPath;
            }

            //取得 arrAppender
            log4net.Appender.IAppender[] arrAppenders = repository.GetAppenders();
            if (arrAppenders != null)
            {
                var targetApder = arrAppenders.FirstOrDefault(p => p.Name == appenderName) as log4net.Appender.RollingFileAppender;
                if (targetApder != null)
                {
                    logPath = targetApder.File;
                }
            }

            return logPath;
        }
        #endregion

        #region 获取记录器
        /// <summary>
        /// 获取记录器
        /// </summary>
        /// <param name="source">soruce</param>
        /// <returns></returns>
        private static ILog GetLogger(Type source)
        {
            if (dic_Loggers.ContainsKey(source))
            {
                return dic_Loggers[source];
            }
            else
            {
                ILog logger = LogManager.GetLogger(repositoryName, source);
                dic_Loggers.TryAdd(source, logger);
                return logger;
            }
        }
        #endregion

        #region Debug
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">源类类型</param>
        /// <param name="strMsg">日志信息</param>
        public static void Debug(Type source, string strMsg)
        {
            //存储库检查
            if (repository == null)
            {
                throw new Exception("Log4NetUtil.Debug->Log4Net Not Init");
            }

            ILog logger = GetLogger(source);
            if (logger.IsDebugEnabled)
            {
                logger.Debug(strMsg);
            }
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">源类</param>
        /// <param name="strMsg">日志信息</param>
        public static void Debug(object source, string strMsg)
        {
            Debug(source.GetType(), strMsg);
        }
        #endregion

        #region Info
        /// <summary>
        /// 日常信息
        /// </summary>
        /// <param name="source">源类类型</param>
        /// <param name="strMsg">日志信息</param>
        public static void Info(Type source, string strMsg)
        {
            //存储库检查
            if (repository == null)
            {
                throw new Exception("Log4NetUtil.Info->Log4Net Not Init");
            }

            ILog logger = GetLogger(source);
            if (logger.IsInfoEnabled)
            {
                logger.Info(strMsg);
            }
        }

        /// <summary>
        /// 日常信息
        /// </summary>
        /// <param name="source">源类</param>
        /// <param name="strMsg">日志信息</param>
        public static void Info(object source, string strMsg)
        {
            Info(source.GetType(), strMsg);
        }
        #endregion

        #region Warn
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">源类类型</param>
        /// <param name="strMsg">日志信息</param>
        public static void Warn(Type source, string strMsg)
        {
            //存储库检查
            if (repository == null)
            {
                throw new Exception("Log4NetUtil.Warn->Log4Net Not Init");
            }

            ILog logger = GetLogger(source);
            if (logger.IsWarnEnabled)
            {
                logger.Warn(strMsg);
            }
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">源类</param>
        /// <param name="strMsg">日志信息</param>
        public static void Warn(object source, string strMsg)
        {
            Warn(source.GetType(), strMsg);
        }
        #endregion

        #region Error
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">源类类型</param>
        /// <param name="strMsg">日志信息</param>
        public static void Error(Type source, string strMsg)
        {
            //存储库检查
            if (repository == null)
            {
                throw new Exception("Log4NetUtil.Error->Log4Net Not Init");
            }

            ILog logger = GetLogger(source);
            if (logger.IsErrorEnabled)
            {
                logger.Error(strMsg);
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">源类</param>
        /// <param name="strMsg">日志信息</param>
        public static void Error(object source, string strMsg)
        {
            Error(source.GetType(), strMsg);
        }
        #endregion

        #region Fatal
        /// <summary>
        /// 严重错误信息
        /// </summary>
        /// <param name="source">源类类型</param>
        /// <param name="strMsg">日志信息</param>
        public static void Fatal(Type source, string strMsg)
        {
            //存储库检查
            if (repository == null)
            {
                throw new Exception("Log4NetUtil.Fatal->Log4Net Not Init");
            }

            ILog logger = GetLogger(source);
            if (logger.IsFatalEnabled)
            {
                logger.Fatal(strMsg);
            }
        }

        /// <summary>
        /// 严重错误信息
        /// </summary>
        /// <param name="source">源类</param>
        /// <param name="strMsg">日志信息</param>
        public static void Fatal(object source, string strMsg)
        {
            Fatal(source.GetType(), strMsg);
        }
        #endregion
    }
}
