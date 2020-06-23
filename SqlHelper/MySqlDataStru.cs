using MySql.Data.MySqlClient;
using System.Collections.Generic;

/******************************************************************
 * Author: miaoxin 
 * Date: 2020-03-05 
 * Content: MySql参数数据结构
 ******************************************************************/

namespace SqlHelper
{
    /// <summary>
    /// MySql参数数据结构
    /// </summary>
    public class MySqlDataStru
    {
        /// <summary>
        /// 列信息
        /// </summary>
        public ColumnInfo columnInfo { get; set; }
        /// <summary>
        /// MySql字段类型
        /// </summary>
        public MySqlDbType dbType { get; set; }
        /// <summary>
        /// 此列是否是String
        /// </summary>
        public bool isString { get; set; }
        /// <summary>
        /// MySql字段数据数组
        /// </summary>
        public List<string> listColValues { get; set; }

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_columnInfo">列信息</param>
        /// <param name="_batchSize">批次数量</param>
        public MySqlDataStru(ColumnInfo _columnInfo,int _batchSize)
        {
            this.isString = true;

            if (_columnInfo != null)
            {
                this.columnInfo = _columnInfo;
                this.dbType = GetMySqlDbType();
            }


            if (_batchSize > 0 && _batchSize != int.MaxValue)
            {
                this.listColValues = new List<string>(_batchSize);
            }
            else
            {
                this.listColValues = new List<string>();
            }
        }
        #endregion

        #region 取得MySql参数类型
        /// <summary>
        /// 取得MySql参数类型
        /// </summary>
        /// <returns></returns>
        public MySqlDbType GetMySqlDbType()
        {
            MySqlDbType mysqlDbType = MySqlDbType.VarString;

            if (columnInfo == null || string.IsNullOrEmpty(columnInfo.ColType))
            {
                return mysqlDbType;
            }
            string colType = columnInfo.ColType.Trim().ToUpper();
            string colLength = columnInfo.ColLength;

            switch (colType)
            {
                //---------------字符串类型----------------
                case "VARCHAR":
                    mysqlDbType = MySqlDbType.VarString;
                    this.isString = true;
                    break;
                case "CHAR":
                    mysqlDbType = MySqlDbType.VarChar;
                    this.isString = true;
                    break;
                case "TINYTEXT":
                    mysqlDbType = MySqlDbType.TinyText;
                    this.isString = true;
                    break;
                case "TEXT":
                    mysqlDbType = MySqlDbType.Text;
                    this.isString = true;
                    break;
                case "MEDIUMTEXT":
                    mysqlDbType = MySqlDbType.MediumText;
                    this.isString = true;
                    break;
                case "LONGTEXT":
                    mysqlDbType = MySqlDbType.LongText;
                    this.isString = true;
                    break;
                //---------------日期和时间类型----------------
                case "DATE":
                    mysqlDbType = MySqlDbType.Date;
                    this.isString = true;
                    break;
                case "TIME":
                    mysqlDbType = MySqlDbType.Time;
                    this.isString = true;
                    break;
                case "YEAR":
                    mysqlDbType = MySqlDbType.Year;
                    this.isString = true;
                    break;
                case "DATETIME":
                    mysqlDbType = MySqlDbType.DateTime;
                    this.isString = true;
                    break;
                case "TIMESTAMP":
                    mysqlDbType = MySqlDbType.Timestamp;
                    this.isString = true;
                    break;
                //---------------数值类型----------------
                case "TINYINT":
                    mysqlDbType = MySqlDbType.Byte;
                    this.isString = false;
                    break;
                case "SMALLINT":
                    mysqlDbType = MySqlDbType.Int16;
                    this.isString = false;
                    break;
                case "MEDIUMINT":
                    mysqlDbType = MySqlDbType.Int24;
                    this.isString = false;
                    break;
                case "INT":
                    mysqlDbType = MySqlDbType.Int32;
                    this.isString = false;
                    break;
                case "INTEGER":
                    mysqlDbType = MySqlDbType.Int32;
                    this.isString = false;
                    break;
                case "BIGINT":
                    mysqlDbType = MySqlDbType.Int64;
                    this.isString = false;
                    break;
                case "FLOAT":
                    mysqlDbType = MySqlDbType.Float;
                    this.isString = false;
                    break;
                case "DOUBLE":
                    mysqlDbType = MySqlDbType.Double;
                    this.isString = false;
                    break;
                case "DECIMAL":
                    mysqlDbType = MySqlDbType.Decimal;
                    this.isString = false;
                    break;
                //---------------二进制类型----------------
                case "TINYBLOB":
                    mysqlDbType = MySqlDbType.TinyBlob;
                    this.isString = false;
                    break;
                case "BLOB":
                    mysqlDbType = MySqlDbType.Blob;
                    this.isString = false;
                    break;
                case "MEDIUMBLOB":
                    mysqlDbType = MySqlDbType.MediumBlob;
                    this.isString = false;
                    break;
                case "LONGBLOB":
                    mysqlDbType = MySqlDbType.LongBlob;
                    this.isString = false;
                    break;
            }

            return mysqlDbType;
        }
        #endregion
    }
}
