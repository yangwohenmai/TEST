using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDB
{
    class DapperAcc
    {
        #region 获取数据库链接
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        /// <param name="connConfig">数据库链接配置结构</param>
        /// <returns></returns>
        public static IDbConnection GetConnection(DbConnStru connConfig)
        {
            IDbConnection conn = null;

            if (string.IsNullOrEmpty(connConfig.connStr))
            {
                return null;
            }

            try
            {
                switch (connConfig.dbType.ToLower().Trim())
                {
                    case "sqlite":
                        conn = new SQLiteConnection(connConfig.connStr);
                        break;
                    default:
                        conn = new SQLiteConnection(connConfig.connStr);
                        break;
                }

                //打开数据库链接
                conn.Open();
            }
            catch (Exception ex)
            {
                //Log4NetUtil.Error(typeof(DapperAcc), "ConnStr:" + connConfig.connStr + " ErrorMsg:" + ex.ToString());
            }
            return conn;
        }
        #endregion
    }
}
