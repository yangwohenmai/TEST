using System.Data;
using System.Collections.Generic;

/******************************************************************
 * Author:  
 * Date: 2020-03-05 
 * Content: MySql参数数据结构
 ******************************************************************/

namespace SqlHelper
{
    /// <summary>
    /// MySql参数数据结构
    /// </summary>
    public class SqlServerDataStru
    {
        /// <summary>
        /// 列信息
        /// </summary>
        public ColumnInfo columnInfo { get; set; }
        /// <summary>
        /// MySql字段类型
        /// </summary>
        public SqlDbType dbType { get; set; }
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
        public SqlServerDataStru(ColumnInfo _columnInfo, int _batchSize)
        {
            if (_columnInfo != null)
            {
                this.columnInfo = _columnInfo;
                this.dbType = GetSqlDbType();
            }

            this.isString = true;

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
        public SqlDbType GetSqlDbType()
        {
            return SqlDbType.VarChar;

            
        }
        #endregion
    }
}
