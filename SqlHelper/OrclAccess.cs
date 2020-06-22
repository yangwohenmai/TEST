using System;
using System.Data;
using CommonLib.Util;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using entDerived.Model.Comm;

/******************************************************************
 * Author: miaoxin 
 * Date: 2017-12-26 
 * Content: Oracle 数据源访问类
 ******************************************************************/

namespace CommonLib.DbAccess
{
    /// <summary>
    /// Oracle 数据源访问类
    /// </summary>
    public class OrclAccess
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
        public OrclAccess(string strConnStr)
        {
            connStr = strConnStr;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connConfig">数据库链接配置结构</param>
        public OrclAccess(ConnConfig connConfig)
        {
            if (connConfig.enum_dbType == EnumDbType.Oracle)
            {
                connStr = connConfig.str_ConnStr;
            }
        }
        #endregion

        #region 获取数据库链接
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        private OracleConnection GetConnection()
        {
            OracleConnection objConn = null;

            if (string.IsNullOrEmpty(connStr))
            {
                return null;
            }

            try
            {
                objConn = new OracleConnection(connStr);
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
            OracleConnection conn = null;
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
            string sql = "select to_char(sysdate,'yyyy-mm-dd hh24:mi:ss') from dual";

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
        public DataSet QueryDs(string sql, OracleParameter[] sqlParams, string tableName, int timeOut, out string errorMsg)
        {
            OracleConnection conn = null;
            OracleCommand cmd = new OracleCommand();
            OracleDataAdapter adapter = new OracleDataAdapter();
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
                    foreach (OracleParameter sp in sqlParams)
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
        public DataTable QueryDt(string sql, OracleParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            OracleConnection conn = null;
            OracleCommand cmd = new OracleCommand();
            OracleDataAdapter adapter = new OracleDataAdapter();
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
                    foreach (OracleParameter sp in sqlParams)
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
        public OracleDataReader QueryDr(string sql, OracleParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            OracleConnection conn = null;
            OracleCommand cmd = new OracleCommand();
            OracleDataReader dr = null;
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
                    foreach (OracleParameter sp in sqlParams)
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
        public OracleDataReader QueryDr(string sql, out string errorMsg)
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
        public object QueryObj(string sql, OracleParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            OracleConnection conn = null;
            OracleCommand cmd = new OracleCommand();
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
                    foreach (OracleParameter sp in sqlParams)
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
        public int Execute(string sql, OracleParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            int intResult = 0;

            OracleConnection conn = null;
            OracleCommand cmd = new OracleCommand();
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
                    foreach (OracleParameter sp in sqlParams)
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
        public int ExecuteTran(string sql, OracleParameter[] sqlParams, int timeOut, out string errorMsg)
        {
            int intResult = 0;
            OracleConnection conn = null;
            OracleCommand cmd = new OracleCommand();
            OracleTransaction sqlTran = null;
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
                    foreach (OracleParameter sp in sqlParams)
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
                try
                {
                    sqlTran.Rollback();
                }
                catch (Exception e)
                {
                    Log4NetUtil.Error(this, "OracleBatchExec->回滚异常 SQL:" + sql + "|*|" + e.ToString());
                }
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
            string strSql = "select * from " + tableName.Trim() + " where 1=2";

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

        #region 批量插入（Insert）
        /// <summary>
        /// 批量插入（Insert）
        /// </summary>
        /// <param name="tableName">目标表名称</param>
        /// <param name="dic_ColumnParaArry">列参数数组</param>
        /// <param name="batchSize">批次中的数量</param>
        /// <param name="timeOut">等待命令执行的时间</param>
        /// <param name="errorMsg">返回的异常信息</param>
        public int OracleBatchInsert(string tableName, Dictionary<string, OrclDataStru> dic_ColumnParaArry, int batchSize, int timeOut, out string errorMsg)
        {
            errorMsg = string.Empty;
            OracleConnection conn = null;
            OracleCommand cmd = new OracleCommand();
            OracleTransaction sqlTran = null;
            StringBuilder sbCmdText = new StringBuilder(); 
            //影响的行数（由于触发器的存在，不一定准确）
            int intResult = 0;
            //列数组
            string[] arr_Columns = null;
            //参数校验
            if (string.IsNullOrEmpty(tableName))
            {
                errorMsg = "tableName 为空";
                return intResult;
            }

            if (dic_ColumnParaArry == null || dic_ColumnParaArry.Count < 1)
            {
                errorMsg = "dic_ColumnParaArry 为空";
                return intResult;
            }
            //取得列数组
            arr_Columns = dic_ColumnParaArry.Keys.ToArray(); 

            //准备Insert语句
            sbCmdText.AppendFormat("INSERT INTO {0}(", tableName);
            sbCmdText.Append(string.Join(",", arr_Columns));
            sbCmdText.Append(") VALUES (");
            sbCmdText.Append(":" + string.Join(",:", arr_Columns));
            sbCmdText.Append(")");

            try
            {
                //取得数据库连接
                conn = GetConnection();
                //OracleCommand
                cmd.Connection = conn;
                //批次中的行数
                cmd.ArrayBindCount = batchSize;
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sbCmdText.ToString();
                //等待命令执行的时间（以秒为单位）,默认值为 300 秒。 
                if (timeOut <= 300)
                {
                    cmd.CommandTimeout = 300;
                }
                else
                {
                    cmd.CommandTimeout = timeOut;
                }

                //创建参数
                foreach (string colName in arr_Columns)
                {
                    OracleDbType dbType = dic_ColumnParaArry[colName].dbType;
                    OracleParameter oraParam = new OracleParameter(colName, dbType);
                    oraParam.Direction = ParameterDirection.Input;

                    if (dic_ColumnParaArry[colName].arrParam != null)
                    {
                        oraParam.Value = dic_ColumnParaArry[colName].arrParam;
                    }
                    else if (dic_ColumnParaArry[colName].arryList != null)
                    {
                        oraParam.Value = dic_ColumnParaArry[colName].arryList.ToArray();
                    }
                    else
                    {
                        errorMsg = "ParaArry 为空";
                        return intResult;
                    }

                    cmd.Parameters.Add(oraParam);
                }

                conn.Open();

                sqlTran = conn.BeginTransaction();
                cmd.Transaction = sqlTran;
                intResult = cmd.ExecuteNonQuery();
                sqlTran.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    sqlTran.Rollback();
                }
                catch (Exception e)
                {
                    Log4NetUtil.Error(this, "OracleBatchInsert->回滚异常 SQL:" + sbCmdText.ToString() + "|*|" + e.ToString());
                }
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
                Log4NetUtil.Error(this, "OracleBatchInsert->SQL:" + sbCmdText.ToString() + "|*|" + errorMsg);
            }

            return intResult;
        }

        /// <summary>
        /// 根据数据类型获取OracleDbType
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static OracleDbType GetOracleDbType(object value)
        {
            OracleDbType dataType = OracleDbType.Varchar2;

            if (value is string[])
            {
                dataType = OracleDbType.Varchar2;
            }
            else if (value is DateTime[])
            {
                //dataType = OracleDbType.Date;
                dataType = OracleDbType.TimeStamp;
            }
            else if (value is int[] || value is short[])
            {
                dataType = OracleDbType.Int32;
            }
            else if (value is long[])
            {
                dataType = OracleDbType.Int64;
            }
            else if (value is decimal[] || value is double[] || value is float[])
            {
                dataType = OracleDbType.Decimal;
            }
            else if (value is Guid[])
            {
                dataType = OracleDbType.Varchar2;
            }
            else if (value is bool[] || value is Boolean[])
            {
                dataType = OracleDbType.Byte;
            }
            else if (value is byte[][])
            {
                dataType = OracleDbType.NClob;
            }
            else if (value is char[])
            {
                dataType = OracleDbType.Char;
            }
            return dataType;
        }
        #endregion

        #region 批量删除（Delete）
        /// <summary>
        /// 批量删除（Delete）
        /// </summary>
        /// <param name="tableName">目标表名称</param>
        /// <param name="dic_ColumnParaArry">列参数数组</param>
        /// <param name="batchSize">批次中的数量</param>
        /// <param name="timeOut">等待命令执行的时间</param>
        /// <param name="errorMsg">返回的异常信息</param>
        public int OracleBatchDel(string tableName, Dictionary<string, OrclDataStru> dic_ColumnParaArry, int batchSize, int timeOut, out string errorMsg)
        {
            errorMsg = string.Empty;
            OracleConnection conn = null;
            OracleCommand cmd = new OracleCommand();
            OracleTransaction sqlTran = null;
            StringBuilder sbCmdText = new StringBuilder();
            //影响的行数（由于触发器的存在，不一定准确）
            int intResult = 0;
            //列数组
            string[] arr_Columns = null;
            //参数校验
            if (string.IsNullOrEmpty(tableName))
            {
                errorMsg = "tableName 为空";
                return intResult;
            }

            if (dic_ColumnParaArry == null || dic_ColumnParaArry.Count < 1)
            {
                errorMsg = "dic_ColumnParaArry 为空";
                return intResult;
            }
            //取得列数组
            arr_Columns = dic_ColumnParaArry.Keys.ToArray();

            //准备Delete语句            
            sbCmdText.Append("DELETE FROM ");
            sbCmdText.Append(tableName);
            sbCmdText.Append(" WHERE ");
            if (arr_Columns.Length == 1)
            {
                sbCmdText.Append(arr_Columns[0]);
                sbCmdText.Append("=:");
                sbCmdText.Append(arr_Columns[0]);
            }
            else
            {
                sbCmdText.Append("1=1");
                foreach (string col in arr_Columns)
                {
                    sbCmdText.Append(" AND ");
                    sbCmdText.Append(col);
                    sbCmdText.Append("=:");
                    sbCmdText.Append(col);
                }
            }

            try
            {
                //取得数据库连接
                conn = GetConnection();
                //OracleCommand
                cmd.Connection = conn;
                //批次中的行数
                if (batchSize > 0)
                {
                    cmd.ArrayBindCount = batchSize;
                }
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sbCmdText.ToString();
                //等待命令执行的时间（以秒为单位）,默认值为 300 秒。 
                if (timeOut <= 300)
                {
                    cmd.CommandTimeout = 300;
                }
                else
                {
                    cmd.CommandTimeout = timeOut;
                }

                //创建参数
                foreach (string colName in arr_Columns)
                {
                    OracleDbType dbType = dic_ColumnParaArry[colName].dbType;
                    OracleParameter oraParam = new OracleParameter(colName, dbType);
                    oraParam.Direction = ParameterDirection.Input;

                    if (dic_ColumnParaArry[colName].arrParam != null)
                    {
                        oraParam.Value = dic_ColumnParaArry[colName].arrParam;
                    }
                    else if (dic_ColumnParaArry[colName].arryList != null)
                    {
                        oraParam.Value = dic_ColumnParaArry[colName].arryList.ToArray();
                    }
                    else
                    {
                        errorMsg = "ParaArry 为空";
                        return intResult;
                    }

                    cmd.Parameters.Add(oraParam);
                }

                conn.Open();

                sqlTran = conn.BeginTransaction();
                cmd.Transaction = sqlTran;
                intResult = cmd.ExecuteNonQuery();
                sqlTran.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    sqlTran.Rollback();
                }
                catch (Exception e)
                {
                    Log4NetUtil.Error(this, "OracleBatchDel->回滚异常 SQL:" + sbCmdText.ToString() + "|*|" + e.ToString());
                }

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
                Log4NetUtil.Error(this, "OracleBatchDel->SQL:" + sbCmdText.ToString() + "|*|" + errorMsg);
            }

            return intResult;
        }
        #endregion

        #region 批量执行语句
        /// <summary>
        /// 批量执行语句
        /// </summary>
        /// <param name="strSql">Sql</param>
        /// <param name="dic_ColumnParaArry">列参数数组</param>
        /// <param name="batchSize">批次中的数量</param>
        /// <param name="timeOut">等待命令执行的时间</param>
        /// <param name="errorMsg">返回的异常信息</param>
        public int OracleBatchExec(string strSql, Dictionary<string, OrclDataStru> dic_ColumnParaArry, int batchSize, int timeOut, out string errorMsg)
        {
            errorMsg = string.Empty;
            OracleConnection conn = null;
            OracleCommand cmd = new OracleCommand();
            OracleTransaction sqlTran = null;

            //影响的行数（由于触发器的存在，不一定准确）
            int intResult = 0;
            //列数组
            string[] arr_Columns = null;
            //参数校验
            if (string.IsNullOrEmpty(strSql))
            {
                errorMsg = "Sql 为空";
                return intResult;
            }

            if (dic_ColumnParaArry == null || dic_ColumnParaArry.Count < 1)
            {
                errorMsg = "dic_ColumnParaArry 为空";
                return intResult;
            }
            //取得列数组
            arr_Columns = dic_ColumnParaArry.Keys.ToArray();

            //准备Insert语句
            //sbCmdText.AppendFormat("INSERT INTO {0}(", tableName);
            //sbCmdText.Append(string.Join(",", arr_Columns));
            //sbCmdText.Append(") VALUES (");
            //sbCmdText.Append(":" + string.Join(",:", arr_Columns));
            //sbCmdText.Append(")");

            try
            {
                //取得数据库连接
                conn = GetConnection();
                //OracleCommand
                cmd.Connection = conn;
                //批次中的行数
                if (batchSize > 0)
                {
                    cmd.ArrayBindCount = batchSize;
                }
                cmd.BindByName = true;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSql;
                //等待命令执行的时间（以秒为单位）,默认值为 300 秒。 
                if (timeOut <= 300)
                {
                    cmd.CommandTimeout = 300;
                }
                else
                {
                    cmd.CommandTimeout = timeOut;
                }

                //创建参数
                foreach (string colName in arr_Columns)
                {
                    OracleDbType dbType = dic_ColumnParaArry[colName].dbType;
                    OracleParameter oraParam = new OracleParameter(colName, dbType);
                    oraParam.Direction = ParameterDirection.Input;

                    if (dic_ColumnParaArry[colName].arrParam != null)
                    {
                        oraParam.Value = dic_ColumnParaArry[colName].arrParam;
                    }
                    else if (dic_ColumnParaArry[colName].arryList != null)
                    {
                        oraParam.Value = dic_ColumnParaArry[colName].arryList.ToArray();
                    }
                    else
                    {
                        errorMsg = "ParaArry 为空";
                        return intResult;
                    }

                    cmd.Parameters.Add(oraParam);
                }

                conn.Open();

                sqlTran = conn.BeginTransaction();
                cmd.Transaction = sqlTran;
                intResult = cmd.ExecuteNonQuery();
                sqlTran.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    sqlTran.Rollback();
                }
                catch (Exception e)
                {
                    Log4NetUtil.Error(this, "OracleBatchExec->回滚异常 SQL:" + strSql + "|*|" + e.ToString());
                }
                
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
                Log4NetUtil.Error(this, "OracleBatchExec->SQL:" + strSql + "|*|" + errorMsg);
            }

            return intResult;
        }
        #endregion
    }
}
