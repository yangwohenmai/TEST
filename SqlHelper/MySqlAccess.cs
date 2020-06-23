using MySql.Data.MySqlClient;
using System;
using System.Data;

/******************************************************************
 * Author: miaoxin 
 * Date: 2019-01-18 
 * Content: MySqlServer 数据源访问类
 ******************************************************************/

namespace SqlHelper
{
    /// <summary>
    /// MySqlServer 数据源访问类
    /// </summary>
    public class MySqlAccess
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        private string connStr = string.Empty;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strConnStr">链接字符串</param>
        public MySqlAccess(string strConnStr)
        {
            connStr = strConnStr;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connConfig">数据库链接配置结构</param>
        public MySqlAccess(ConnConfig connConfig)
        {
            if (connConfig.enum_dbType == EnumDbType.MySql)
            {
                connStr = connConfig.str_ConnStr;
            }
        }
        #endregion

        #region 获取数据库链接
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        private MySqlConnection GetConnection()
        {
            MySqlConnection objConn = null;

            if (string.IsNullOrEmpty(connStr))
            {
                return null;
            }

            try
            {
                objConn = new MySqlConnection(connStr);
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
            MySqlConnection conn = null;
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

        #region 取得系统时间
        /// <summary>
        /// 取得系统时间
        /// </summary>
        /// <returns></returns>
        public string GetSysDateTime()
        {
            string resutl = string.Empty;
            string errorMsg = string.Empty;
            string sql = "SELECT DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s') as 'sysdate'";

            resutl = QueryObj(sql, out errorMsg) as string;
            return resutl;
        }
        #endregion

        #region QueryDs  执行查询操作，查询结果保存至 DataSet 中
        /// <summary>
        /// 执行查询操作，查询结果保存至 DataSet 中。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlParams">参数数组</param>
        /// <param name="tableName">指定填充的TableName</param>
        /// <param name="timeOut">等待命令执行的时间（以秒为单位）,默认值为 30 秒。</param>  
        /// <param name="errorMsg">返回的异常信息</param>
        public DataSet QueryDs(string sql, MySqlParameter[] sqlParams, string tableName, int timeOut, out string errorMsg)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataSet ds = new DataSet();
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (sqlParams != null)
                {
                    foreach (MySqlParameter sp in sqlParams)
                    {
                        cmd.Parameters.Add(sp);
                    }
                }

                //等待命令执行的时间（以秒为单位）,默认值为 30 秒。 
                if (timeOut <= 30)
                {
                    cmd.CommandTimeout = 30;
                }
                else
                {
                    cmd.CommandTimeout = timeOut;
                }

                adapter.SelectCommand = cmd;
                conn.Open();

                if (!string.IsNullOrEmpty(tableName))
                {
                    adapter.Fill(ds, tableName);
                }
                else
                {
                    adapter.Fill(ds);
                }

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

            //记录日志
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetUtil.Error(this, "SQL:" + sql + "|*|ErrorMsg:" + errorMsg);
            }

            return ds;
        }

        /// <summary>
        /// 执行查询操作，查询结果保存至 DataSet 中。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="tableName">指定填充的TableName</param>
        /// <param name="errorMsg">返回的异常信息</param>
        public DataSet QueryDs(string sql, string tableName, out string errorMsg)
        {
            return QueryDs(sql, null, tableName, 120, out errorMsg);
        }

        /// <summary>
        /// 执行查询操作，查询结果保存至 DataSet 中。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="errorMsg">返回的异常信息</param>
        public DataSet QueryDs(string sql, out string errorMsg)
        {
            return QueryDs(sql, null, string.Empty, 120, out errorMsg);
        }
        #endregion

        #region QueryDt  执行查询操作，查询结果保存至 DataTable 中
        /// <summary>
        /// 执行查询操作，查询结果保存至 DataTable 中。
        /// </summary>    
        /// <param name="sql">sql语句</param>
        /// <param name="sqlParams">参数数组</param>
        /// <param name="timeOut">等待命令执行的时间（以秒为单位）,默认值为 30 秒。</param>  
        /// <param name="errorMsg">返回的异常信息</param>
        public DataTable QueryDt(string sql, MySqlParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dt = new DataTable();
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (sqlParams != null)
                {
                    foreach (MySqlParameter sp in sqlParams)
                    {
                        cmd.Parameters.Add(sp);
                    }
                }

                //等待命令执行的时间（以秒为单位）,默认值为 30 秒。 
                if (timeOut <= 30)
                {
                    cmd.CommandTimeout = 30;
                }
                else
                {
                    cmd.CommandTimeout = timeOut;
                }

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

            //记录日志
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetUtil.Error(this, "SQL:" + sql + "|*|ErrorMsg:" + errorMsg);
            }

            return dt;
        }

        /// <summary>
        /// 执行查询操作，查询结果保存至 DataTable 中。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="errorMsg">返回的异常信息</param>
        public DataTable QueryDt(string sql, out string errorMsg)
        {
            return QueryDt(sql, null, 120, out errorMsg);
        }
        #endregion

        #region QueryDr  执行查询操作，返回 MySqlDataReader 对象
        /// <summary>
        /// 执行查询操作，返回 MySqlDataReader 对象。
        /// </summary>           
        /// <param name="sql">sql语句</param>
        /// <param name="sqlParams">参数数组</param>
        /// <param name="timeOut">等待命令执行的时间（以秒为单位）,默认值为 30 秒。</param>  
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns>MySqlDataReader 对象</returns>
        public MySqlDataReader QueryDr(string sql, MySqlParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader dr = null;
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (sqlParams != null)
                {
                    foreach (MySqlParameter sp in sqlParams)
                    {
                        cmd.Parameters.Add(sp);
                    }
                }

                //等待命令执行的时间（以秒为单位）,默认值为 30 秒。 
                if (timeOut <= 30)
                {
                    cmd.CommandTimeout = 30;
                }
                else
                {
                    cmd.CommandTimeout = timeOut;
                }

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

            //记录日志
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetUtil.Error(this, "SQL:" + sql + "|*|ErrorMsg:" + errorMsg);
            }

            return dr;
        }

        /// <summary>
        /// 执行查询操作，返回 MySqlDataReader 对象。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns>MySqlDataReader 对象</returns>
        public MySqlDataReader QueryDr(string sql, out string errorMsg)
        {
            return QueryDr(sql, null, 120, out errorMsg);
        }
        #endregion

        #region QueryObj  执行查询操作，返回 结果集的第一行第一列 (object类型)
        /// <summary>
        /// 执行查询操作，返回结果集的第一行第一列 (object类型)
        /// </summary>           
        /// <param name="sql">sql语句</param>
        /// <param name="sqlParams">参数数组</param>
        /// <param name="timeOut">等待命令执行的时间（以秒为单位）,默认值为 30 秒。</param>  
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns>object 对象</returns>
        public object QueryObj(string sql, MySqlParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();
            object obj = null;
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (sqlParams != null)
                {
                    foreach (MySqlParameter sp in sqlParams)
                    {
                        cmd.Parameters.Add(sp);
                    }
                }

                //等待命令执行的时间（以秒为单位）,默认值为 30 秒。 
                if (timeOut <= 30)
                {
                    cmd.CommandTimeout = 30;
                }
                else
                {
                    cmd.CommandTimeout = timeOut;
                }

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

            //记录日志
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetUtil.Error(this, "SQL:" + sql + "|*|ErrorMsg:" + errorMsg);
            }

            return obj;
        }

        /// <summary>
        /// 执行查询操作，返回结果集的第一行第一列 (Object类型)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns>Object 对象</returns>
        public object QueryObj(string sql, out string errorMsg)
        {
            return QueryObj(sql, null, 120, out errorMsg);
        }
        #endregion

        #region Execute  执行SQL语句，返回受影响的行数 (无事物)
        /// <summary>
        /// 执行SQL语句，返回受影响的行数 (无事物)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlParams">参数数组</param>
        /// <param name="timeOut">等待命令执行的时间（以秒为单位）,默认值为 30 秒。</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns>受影响的行数</returns>
        public int Execute(string sql, MySqlParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            int intResult = 0;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (sqlParams != null)
                {
                    foreach (MySqlParameter sp in sqlParams)
                    {
                        cmd.Parameters.Add(sp);
                    }
                }

                //等待命令执行的时间（以秒为单位）,默认值为 30 秒。 
                if (timeOut <= 30)
                {
                    cmd.CommandTimeout = 30;
                }
                else
                {
                    cmd.CommandTimeout = timeOut;
                }

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

            //记录日志
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetUtil.Error(this, "SQL:" + sql + "|*|ErrorMsg:" + errorMsg);
            }

            return intResult;
        }

        /// <summary>
        /// 执行SQL语句，返回受影响的行数。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="timeOut">等待命令执行的时间（以秒为单位）,默认值为 30 秒。</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns>受影响的行数</returns>
        public int Execute(string sql, int timeOut, out string errorMsg)
        {
            return Execute(sql, null, timeOut, out errorMsg);
        }

        /// <summary>
        /// 执行SQL语句，返回受影响的行数。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns>受影响的行数</returns>
        public int Execute(string sql, out string errorMsg)
        {
            return Execute(sql, null, 120, out errorMsg);
        }
        #endregion

        #region ExecuteTran  执行SQL语句，返回受影响的行数 (带事物)
        /// <summary>
        /// 执行SQL语句，返回受影响的行数(带事物)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlParams">参数数组</param>
        /// <param name="timeOut">等待命令执行的时间（以秒为单位）,默认值为 30 秒。</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteTran(string sql, MySqlParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            int intResult = 0;
            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();
            MySqlTransaction sqlTran = null;
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (sqlParams != null)
                {
                    foreach (MySqlParameter sp in sqlParams)
                    {
                        cmd.Parameters.Add(sp);
                    }
                }

                //等待命令执行的时间（以秒为单位）,默认值为 30 秒。 
                if (timeOut <= 30)
                {
                    cmd.CommandTimeout = 30;
                }
                else
                {
                    cmd.CommandTimeout = timeOut;
                }

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

            //记录日志
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetUtil.Error(this, "SQL:" + sql + "|*|ErrorMsg:" + errorMsg);
            }

            return intResult;
        }

        /// <summary>
        /// 执行SQL语句，返回受影响的行数。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteTran(string sql, out string errorMsg)
        {
            return ExecuteTran(sql, null, 120, out errorMsg);
        }
        #endregion
    }
}
