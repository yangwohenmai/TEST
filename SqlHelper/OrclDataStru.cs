using entDerived.Model.Mission;
using Oracle.ManagedDataAccess.Client;
using System.Collections;

/******************************************************************
 * Author: miaoxin 
 * Date: 2019-02-19 
 * Content: Oracle参数数据结构
 ******************************************************************/

namespace CommonLib.DbAccess
{
    /// <summary>
    /// Oracle参数数据结构
    /// </summary>
    public class OrclDataStru
    {
        /// <summary>
        /// Oracle字段类型
        /// </summary>
        public OracleDbType dbType { get; set; }
        /// <summary>
        /// Oracle字段数据数组
        /// </summary>
        public object arrParam { get; set; }
        /// <summary>
        /// Oracle字段数据数组
        /// </summary>
        public ArrayList arryList { get; set; }

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public OrclDataStru()
        {
            dbType = OracleDbType.Varchar2;
        }
        #endregion

        #region 取得Oracle参数类型
        /// <summary>
        /// 取得Oracle参数类型
        /// </summary>
        /// <param name="columnInfo"></param>
        /// <returns></returns>
        public static OracleDbType GetOrclDbType(ColumnInfo columnInfo)
        {
            OracleDbType oracleDbType = OracleDbType.Varchar2;

            if (columnInfo == null || string.IsNullOrEmpty(columnInfo.ColType))
            {
                return oracleDbType; 
            }
            string colType = columnInfo.ColType.Trim().ToUpper();
            string colLength = columnInfo.ColLength;

            switch (colType)
            {
                case "VARCHAR":
                    oracleDbType = OracleDbType.Varchar2;
                    break;
                case "VARCHAR2":
                    oracleDbType = OracleDbType.Varchar2;
                    break;
                case "NVARCHAR2":
                    oracleDbType = OracleDbType.NVarchar2;
                    break;
                case "CHAR":
                    oracleDbType = OracleDbType.Char;
                    break;
                case "NCHAR":
                    oracleDbType = OracleDbType.NChar;
                    break;
                case "CLOB":
                    oracleDbType = OracleDbType.Clob;
                    break;
                case "NCLOB":
                    oracleDbType = OracleDbType.NClob;
                    break;
                case "BLOB":
                    oracleDbType = OracleDbType.Blob;
                    break;
                case "NUMBER":
                    if (colLength.IndexOf(",") > 0)
                    {
                        //string[] arrLength = colLength.Split(",");
                        //if (arrLength.Length == 2 && arrLength[1].Trim() == "0")
                        //{
                        //    oracleDbType = OracleDbType.Int64;
                        //}
                        //else
                        //{
                        //    oracleDbType = OracleDbType.Decimal;
                        //}
                        oracleDbType = OracleDbType.Decimal;
                    }
                    else
                    {
                        oracleDbType = OracleDbType.Int64;
                    }                    
                    break;
                case "BINARY_FLOAT":
                    oracleDbType = OracleDbType.BinaryFloat;
                    break;
                case "BINARY_DOUBLE":
                    oracleDbType = OracleDbType.BinaryDouble;
                    break;
                case "DATE":
                    oracleDbType = OracleDbType.Date;
                    break;
                case "TIMESTAMP":
                    oracleDbType = OracleDbType.TimeStamp;
                    break;
            }

            return oracleDbType;
        }
        #endregion
    }
}
