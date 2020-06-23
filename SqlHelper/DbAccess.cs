using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;

/******************************************************************
 * Author: miaoxin 
 * Date: 2017-12-26 
 * Content: 通用数据库访问基类
 ******************************************************************/

namespace SqlHelper
{
    /// <summary>
    /// 通用数据库访问基类
    /// </summary>
    public class DbAccess
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        private string connStr = string.Empty;
        /// <summary>
        /// 数据库类型枚举
        /// </summary>
        private EnumDbType dbType = EnumDbType.SqlServer;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connConfig">数据库链接配置结构</param>
        public DbAccess(ConnConfig connConfig)
        {
            connStr = connConfig.str_ConnStr;
            dbType = connConfig.enum_dbType;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="enumDbType">数据库类型枚举</param>
        /// <param name="strConnStr">链接字符串</param>
        public DbAccess(EnumDbType enumDbType, string strConnStr)
        {
            connStr = strConnStr;
            dbType = enumDbType;
        }
        #endregion

        #region 获取数据库类型
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="strDbType">字符串数据库类型</param>
        /// <returns></returns>
        public static EnumDbType GetDbType(string strDbType)
        {
            EnumDbType edt = EnumDbType.Null;
            strDbType = strDbType.ToLower();

            if (strDbType.IndexOf("sqlserver") >= 0)
            {
                edt = EnumDbType.SqlServer;
            }
            else if (strDbType.IndexOf("oracle") >= 0)
            {
                edt = EnumDbType.Oracle;
            }
            else if (strDbType.IndexOf("mysql") >= 0)
            {
                edt = EnumDbType.MySql;
            }
            else if (strDbType.IndexOf("db2") >= 0)
            {
                edt = EnumDbType.DB2;
            }
            else if (strDbType.IndexOf("sqlite") >= 0)
            {
                edt = EnumDbType.SQLite;
            }
            else
            {
                edt = EnumDbType.Null;
            }
            return edt;
        }
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="edt">枚举数据库类型</param>
        /// <returns></returns>
        public static string GetDbTypeStr(EnumDbType edt)
        {
            string result = string.Empty;
            switch (edt)
            {
                case EnumDbType.SqlServer:
                    result = "sqlserver";
                    break;
                case EnumDbType.Oracle:
                    result = "oracle";
                    break;
                case EnumDbType.MySql:
                    result = "mysql";
                    break;
                case EnumDbType.DB2:
                    result = "db2";
                    break;
                case EnumDbType.SQLite:
                    result = "sqlite";
                    break;
                default:
                    result = string.Empty;
                    break;
            }
            return result;
        }
        #endregion

        #region 获取数据库链接
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        private DbConnection GetConnection()
        {
            DbConnection objConn = null;

            if (string.IsNullOrEmpty(connStr))
            {
                return null;
            }

            try
            {
                switch (dbType)
                {
                    case EnumDbType.SqlServer:
                        objConn = new SqlConnection(connStr);
                        break;
                    case EnumDbType.Oracle:
                        objConn = new OracleConnection(connStr);
                        break;
                    case EnumDbType.MySql:
                        objConn = new MySqlConnection(connStr);
                        break;
                    case EnumDbType.SQLite:
                        objConn = new SQLiteConnection(connStr);
                        break;
                    default:
                        objConn = new SqlConnection(connStr);
                        break;
                }
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
            DbConnection conn = null;
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

        #region 获取数据适配器
        /// <summary>
        /// 获取数据适配器
        /// </summary>
        private DbDataAdapter GetDataAdapter()
        {
            DbDataAdapter objDa = null;

            try
            {
                switch (dbType)
                {
                    case EnumDbType.SqlServer:
                        objDa = new SqlDataAdapter();
                        break;
                    case EnumDbType.Oracle:
                        objDa = new OracleDataAdapter();
                        break;
                    case EnumDbType.MySql:
                        objDa = new MySqlDataAdapter();
                        break;
                    case EnumDbType.SQLite:
                        objDa = new SQLiteDataAdapter();
                        break;
                    default:
                        objDa = new SqlDataAdapter();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objDa;
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
        public DataSet QueryDs(string sql, DbParameter[] sqlParams, string tableName, int timeOut, out string errorMsg)
        {
            DbConnection conn = null;
            DbCommand cmd = null;
            DbDataAdapter adapter = null;
            DataSet ds = new DataSet();
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();
                //获取数据适配器
                adapter = GetDataAdapter();

                cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (sqlParams != null)
                {
                    foreach (DbParameter sp in sqlParams)
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
        public DataTable QueryDt(string sql, DbParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            DbConnection conn = null;
            DbCommand cmd = null;
            DbDataAdapter adapter = null;
            DataTable dt = new DataTable();
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();
                //获取数据适配器
                adapter = GetDataAdapter();

                cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (sqlParams != null)
                {
                    foreach (DbParameter sp in sqlParams)
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

        #region QueryDr  执行查询操作，返回 SqlDataReader 对象
        /// <summary>
        /// 执行查询操作，返回 SqlDataReader 对象。
        /// </summary>           
        /// <param name="sql">sql语句</param>
        /// <param name="sqlParams">参数数组</param>
        /// <param name="timeOut">等待命令执行的时间（以秒为单位）,默认值为 30 秒。</param>  
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns>SqlDataReader 对象</returns>
        public DbDataReader QueryDr(string sql, DbParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            DbConnection conn = null;
            DbCommand cmd = null;
            DbDataReader dr = null;
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();

                cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (sqlParams != null)
                {
                    foreach (DbParameter sp in sqlParams)
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
        /// 执行查询操作，返回 SqlDataReader 对象。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns>SqlDataReader 对象</returns>
        public DbDataReader QueryDr(string sql, out string errorMsg)
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
        public object QueryObj(string sql, DbParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            DbConnection conn = null;
            DbCommand cmd = null;
            object obj = null;
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();

                cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (sqlParams != null)
                {
                    foreach (DbParameter sp in sqlParams)
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
        public int Execute(string sql, DbParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            int intResult = 0;
            DbConnection conn = null;
            DbCommand cmd = null;
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();

                cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (sqlParams != null)
                {
                    foreach (DbParameter sp in sqlParams)
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
        public int ExecuteTran(string sql, DbParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            int intResult = 0;
            DbConnection conn = null;
            DbCommand cmd = null;
            DbTransaction sqlTran = null;
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();

                cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                if (sqlParams != null)
                {
                    foreach (DbParameter sp in sqlParams)
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
