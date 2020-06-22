using System;
using System.Data.SQLite;
using System.Data;

/******************************************************************
 * Author: miaoxin 
 * Date: 2018-10-31
 * Content: Sqlite访问类
 ******************************************************************/

namespace CommonLib.DbAccess
{
    /// <summary>
    /// Sqlite访问类
    /// </summary>
    public class SqliteAccess
    {
        /// <summary>
        /// 数据库链接字符串(Data Source=d:\test.db3;Pooling=true;FailIfMissing=false)
        /// </summary>
        private string connStr = string.Empty;
        private string connStrFormat = @"Data Source={0};Pooling=true;FailIfMissing=false";

        /// <summary>
        /// 静态锁，避免死锁
        /// </summary>
        public static object padlock = new object();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strDbPath">Sqlite路径</param>
        public SqliteAccess(string strDbPath)
        {
            connStr = string.Format(connStrFormat, strDbPath);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connConfig">数据库链接配置结构</param>
        public SqliteAccess(ConnConfig connConfig)
        {
            if (connConfig.enum_dbType == EnumDbType.SQLite)
            {
                connStr = connConfig.str_ConnStr;
            }
        }
        #endregion

        #region 获取链接
        /// <summary>
        /// 获取链接
        /// </summary>
        /// <returns>SQLiteConnection 对象</returns>
        private SQLiteConnection GetConnection()
        {
            SQLiteConnection objConn = null;

            if (string.IsNullOrEmpty(connStr))
            {
                return null;
            }

            try
            {
                objConn = new SQLiteConnection(connStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objConn;
        }
        #endregion

        #region 测试数据库连接
        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <returns></returns>
        public bool TestConn()
        {
            bool result = true;
            SQLiteConnection conn = null;
            try
            {
                conn = GetConnection();
                if (conn == null)
                {
                    return false;
                }
                conn.Open();
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return result;
        }
        #endregion

        #region QueryDt  执行查询操作，查询结果保存至 DataTable 中
        /// <summary>
        /// 执行查询操作，查询结果保存至 DataTable 中。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns></returns>
        public DataTable QueryDt(string sql, out string errorMsg)
        {
            SQLiteConnection conn = null;
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            DataTable dt = new DataTable();
            errorMsg = string.Empty;

            lock (padlock)
            {
                try
                {
                    //取得数据库连接
                    conn = GetConnection();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    adapter.SelectCommand = cmd;
                    conn.Open();
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.ToString();
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                    cmd.Dispose();
                    adapter.Dispose();
                }
            }            

            //记录日志
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetUtil.Error(this, "SQL:" + sql + " ConnStr:" + connStr + "|*|" + errorMsg);
            }

            return dt;
        }
        #endregion

        #region QueryDr  执行查询操作，返回 SQLiteDataReader 对象
        /// <summary>
        /// 执行查询操作，返回 SQLiteDataReader 对象。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns></returns>
        public SQLiteDataReader QueryDr(string sql, out string errorMsg)
        {
            SQLiteConnection conn = null;
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataReader dr = null;
            errorMsg = string.Empty;

            lock (padlock)
            {
                try
                {
                    //取得数据库连接
                    conn = GetConnection();

                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.ToString();
                }
                finally
                {
                    cmd.Dispose();
                }
            }

            //记录日志
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetUtil.Error(this, "SQL:" + sql + " ConnStr:" + connStr + "|*|" + errorMsg);
            }

            return dr;
        }
        #endregion

        #region QueryObj  执行查询操作，返回 结果集的第一行第一列 (Object类型)
        /// <summary>
        /// 执行查询操作，返回结果集的第一行第一列 (object类型)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns></returns>
        public object QueryObj(string sql, out string errorMsg)
        {
            SQLiteConnection conn = null;
            SQLiteCommand cmd = new SQLiteCommand();
            object obj = null;
            errorMsg = string.Empty;

            lock (padlock)
            {
                try
                {
                    //取得数据库连接
                    conn = GetConnection();

                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    obj = cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    errorMsg = ex.ToString();
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                    cmd.Dispose();
                }
            }

            //记录日志
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetUtil.Error(this, "SQL:" + sql + " ConnStr:" + connStr + "|*|" + errorMsg);
            }

            return obj;
        }
        #endregion

        #region Execute  执行SQL语句，返回受影响的行数 (无事物)
        /// <summary>
        /// 执行SQL语句，返回受影响的行数。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns></returns>
        public int Execute(string sql, out string errorMsg)
        {
            int intResult = 0;
            SQLiteConnection conn = null;
            SQLiteCommand cmd = new SQLiteCommand();
            errorMsg = string.Empty;

            lock (padlock)
            {
                try
                {
                    //取得数据库连接
                    conn = GetConnection();

                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    intResult = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    errorMsg = ex.ToString();
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                    cmd.Dispose();
                }
            }

            //记录日志
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetUtil.Error(this, "SQL:" + sql + " ConnStr:" + connStr + "|*|" + errorMsg);
            }

            return intResult;
        }
        #endregion

        #region ExecuteTran  执行SQL语句，返回受影响的行数 (批处理事物)
        /// <summary>
        /// 执行SQL语句，返回受影响的行数。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns></returns>
        public int ExecuteTran(string sql, out string errorMsg)
        {
            int intResult = 0;

            SQLiteConnection conn = null;
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction sqlTran = null;

            errorMsg = string.Empty;

            lock (padlock)
            {
                try
                {
                    //取得数据库连接
                    conn = GetConnection();

                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();

                    sqlTran = conn.BeginTransaction();
                    cmd.Transaction = sqlTran;
                    intResult = cmd.ExecuteNonQuery();
                    sqlTran.Commit();
                }
                catch (Exception ex)
                {
                    sqlTran.Rollback();
                    errorMsg = ex.ToString();
                }
                finally
                {
                    if (sqlTran != null)
                    {
                        sqlTran.Dispose();
                    }
                    if (conn != null)
                    {
                        conn.Close();
                    }
                    cmd.Dispose();
                }
            }

            //记录日志
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetUtil.Error(this, "SQL:" + sql + " ConnStr:" + connStr + "|*|" + errorMsg);
            }

            return intResult;
        }
        #endregion
    }
}
