
/******************************************************************
 * Author: 
 * Date: 
 * Content: 列配置结构
 ******************************************************************/

namespace SqlHelper
{
    /// <summary>
    /// 列配置结构
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        /// 列名称
        /// </summary>
        public string ColName { get; set; }
        /// <summary>
        /// 列中文
        /// </summary>
        public string ColName_CN { get; set; }
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPk { get; set; }
        /// <summary>
        /// 是否不为Null
        /// </summary>
        public bool IsNotNull { get; set; }
        /// <summary>
        /// 是否忽略
        /// </summary>
        public bool IsIgnore { get; set; }
        /// <summary>
        /// 列类型
        /// </summary>
        public string ColType { get; set; }
        /// <summary>
        /// 列长度
        /// </summary>
        public string ColLength { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }
    }
}
