using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMapping
{
    class Program
    {
        static void Main(string[] args)
        {
            string remote = @"\\10.15.98.162\CentralDB\CentralDB_Stock";
            string local = "p:";
            string username = "dba";
            string password = "dba";

            //自己的测试环境
            //string remote = @"\\10.15.23.185\wq";
            //string local = "p:";
            //string username = "administrator";
            //string password = "yssj";

            WEB.DeleteMap(local);

            bool a = WEB.CreateMap(username, password, remote, local);
            WEB.DeleteMap(local);
            bool a1 = WEB.CreateMap(username, password, remote, local);
        }
    }
}
