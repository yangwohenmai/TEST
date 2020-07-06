using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace SqlHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string ErrorMsg = string.Empty;
            SqlAccess sqlaccess = new SqlAccess("DataBaseLink");
            sqlaccess.SqlBulkCopyInsert(new DataTable(), "TableName", Convert.ToInt32("BatchSize"), out ErrorMsg);
        }
    }
}
