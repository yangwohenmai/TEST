/******************************************************************
 * Author: miaoxin 
 * Date: 2018-10-20 
 * Content: 数据库连接配置结构
 ******************************************************************/

namespace entDerived.Model.Comm
{
    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum EnumDbType
    {
        SqlServer = 1, Oracle = 2, MySql = 3, DB2 = 4, SQLite = 5, Null = 100
    }

    /// <summary>
    /// 数据库链接配置结构
    /// </summary>
    public class ConnConfig
    {
        /// <summary>
        /// 链接标识
        /// </summary>
        public string str_Name { get; set; }
        /// <summary>
        /// 链接标识
        /// </summary>
        public string str_DbName { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public EnumDbType enum_dbType { get; set; }
        /// <summary>
        /// 链接字符串（原始）
        /// </summary>
        public string str_ConnStr_Original { get; set; }
        /// <summary>
        /// 链接字符串（解密后）
        /// </summary>
        public string str_ConnStr { get; set; }
        /// <summary>
        /// 链接字符串是否被加密
        /// </summary>
        public bool bool_IsEncrypt { get; set; }
        /// <summary>
        /// 数据库编码
        /// </summary>
        public string str_Encoding { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string str_Describe { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string str_Ver { get; set; }
    }
}
