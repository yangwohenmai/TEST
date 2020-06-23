using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using entDerived.Model.Comm;

/******************************************************************
 * Author: miaoxin 
 * Date: 2018-10-31 
 * Content: SqlServer 数据源访问类
 ******************************************************************/

namespace CommonLib.DbAccess
{
    /// <summary>
    /// SqlServer 数据源访问类
    /// </summary>
    public class SqlAccess
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
        public SqlAccess(string strConnStr)
        {
            connStr = strConnStr;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connConfig">数据库链接配置结构</param>
        public SqlAccess(ConnConfig connConfig)
        {
            if (connConfig.enum_dbType == EnumDbType.SqlServer)
            {
                connStr = connConfig.str_ConnStr;
            }
        }
        #endregion

        #region 获取数据库链接
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        private SqlConnection GetConnection()
        {
            SqlConnection objConn = null;

            if (string.IsNullOrEmpty(connStr))
            {
                return null;
            }

            try
            {
                objConn = new SqlConnection(connStr);
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
            SqlConnection conn = null;
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
            string sql = "select convert(varchar(20),getdate(),120)";

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
        public DataSet QueryDs(string sql, SqlParameter[] sqlParams, string tableName, int timeOut, out string errorMsg)
        {
            SqlConnection conn = null;
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();
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
                    foreach (SqlParameter sp in sqlParams)
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
        public DataTable QueryDt(string sql, SqlParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            SqlConnection conn = null;
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();
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
                    foreach (SqlParameter sp in sqlParams)
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
        public SqlDataReader QueryDr(string sql, SqlParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            SqlConnection conn = null;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr = null;
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
                    foreach (SqlParameter sp in sqlParams)
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
        public SqlDataReader QueryDr(string sql, out string errorMsg)
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
        public object QueryObj(string sql, SqlParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            SqlConnection conn = null;
            SqlCommand cmd = new SqlCommand();
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
                    foreach (SqlParameter sp in sqlParams)
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
        public int Execute(string sql, SqlParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            int intResult = 0;

            SqlConnection conn = null;
            SqlCommand cmd = new SqlCommand();
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
                    foreach (SqlParameter sp in sqlParams)
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
        public int ExecuteTran(string sql, SqlParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            int intResult = 0;
            SqlConnection conn = null;
            SqlCommand cmd = new SqlCommand();
            SqlTransaction sqlTran = null;
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
                    foreach (SqlParameter sp in sqlParams)
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

        #region 取得数据表结构
        /// <summary>
        /// 取得数据表结构
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="errorMsg">返回的异常信息</param>
        /// <returns>DataTable结构</returns>
        public DataTable GetTableSchema(string tableName, out string errorMsg)
        {
            errorMsg = string.Empty;
            string strSql = "select * from " + tableName.Trim() + " where 1=2;";

            DataSet ds = QueryDs(strSql, tableName, out errorMsg);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                return null;
            }
            else
            {
                return ds.Tables[tableName.Trim()];
            }
        }
        #endregion

        #region 批量插入(DataTable 源)
        /// <summary>
        /// 批量插入(DataTable 源,带事务)
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="tableName">目标表名称</param>
        /// <param name="dic_ColumnMapping">源和目标的列映射</param>
        /// <param name="batchSize">每一提交批次中的行数</param>
        /// <param name="timeOut">等待命令执行的时间</param>
        /// <param name="isNotify">是否每批次通知</param>
        /// <param name="errorMsg">返回的异常信息</param>
        public void SqlBulkCopyInsert(DataTable dt, string tableName, Dictionary<string, string> dic_ColumnMapping, int batchSize, int timeOut, bool isNotify, out string errorMsg)
        {
            SqlConnection conn = null;
            SqlBulkCopy sqlBulkCopy = null;
            SqlTransaction sqlTran = null;
            errorMsg = string.Empty;

            try
            {
                //取得数据库连接
                conn = GetConnection();
                conn.Open();

                sqlTran = conn.BeginTransaction();
                //sqlBulkCopy = new SqlBulkCopy(conn);
                sqlBulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.KeepNulls, sqlTran);

                //目标表名称
                sqlBulkCopy.DestinationTableName = tableName.Trim();

                //源和目标的列映射
                if (dic_ColumnMapping != null && dic_ColumnMapping.Count > 0)
                {
                    foreach (string strKey in dic_ColumnMapping.Keys)
                    {
                        sqlBulkCopy.ColumnMappings.Add(strKey, dic_ColumnMapping[strKey]);
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sqlBulkCopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                    }
                }

                //每一批次中的行数
                if (batchSize > 1)
                {
                    sqlBulkCopy.BatchSize = batchSize;
                }

                //等待命令执行的时间（以秒为单位）,默认值为 300 秒。 
                if (timeOut <= 300)
                {
                    sqlBulkCopy.BulkCopyTimeout = 300;
                }
                else
                {
                    sqlBulkCopy.BulkCopyTimeout = timeOut;
                }

                //是否按批次通知
                if (isNotify)
                {
                    sqlBulkCopy.NotifyAfter = sqlBulkCopy.BatchSize;
                    sqlBulkCopy.SqlRowsCopied += new SqlRowsCopiedEventHandler(BulkCopy_SqlRowsCopied_Notify);
                }

                if (dt != null && dt.Rows.Count != 0)
                {
                    sqlBulkCopy.WriteToServer(dt);
                    sqlTran.Commit();
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
                sqlTran.Rollback();
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

                if (sqlBulkCopy != null)
                {
                    sqlBulkCopy.Close();
                }
            }

            //记录日志
            if (!string.IsNullOrEmpty(errorMsg))
            {
                Log4NetUtil.Error(this, "SqlBulkCopyInsert->"+errorMsg);
            }
        }

        /// <summary>
        /// 批次处理通知事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BulkCopy_SqlRowsCopied_Notify(object sender, SqlRowsCopiedEventArgs e)
        {
            string strTableName = string.Empty;
            SqlBulkCopy sqlBulkCopy = sender as SqlBulkCopy;
            if (sqlBulkCopy != null)
            {
                strTableName = sqlBulkCopy.DestinationTableName;
            }
            Log4NetUtil.Info(this, strTableName + " 当前已复制行数：" + e.RowsCopied.ToString());
        }

        /// <summary>
        /// 批量插入(DataTable 源)
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="tableName">目标表名称</param>
        /// <param name="batchSize">每一批次中的行数</param>
        /// <param name="isNotify">是否每批次通知</param>
        /// <param name="errorMsg">返回的异常信息</param>
        public void SqlBulkCopyInsert(DataTable dt, string tableName, int batchSize, out string errorMsg)
        {
            SqlBulkCopyInsert(dt, tableName, null, batchSize, 300, false, out errorMsg);
        }
        #endregion
    }
}
