using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JSon
{
    class Program
    {
        static void Main(string[] args)
        {
            

            string unicodeString = "Type:鍥藉�";
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(unicodeString);
            String decodedString = utf8.GetString(encodedBytes);

            Encoding big5 = Encoding.GetEncoding("UTF-8");
            Encoding gb2312 = Encoding.GetEncoding("gb2312");

            byte[] big5b= big5.GetBytes(unicodeString);
            //关键也就是这句了
            byte[] gb2312b= Encoding.Convert(big5,gb2312,big5b);

            string strGb2312 = gb2312.GetString(gb2312b);
            


            string a12 = "[{\"Type\":\"国债\",\"Code\":\"08031000001HJZ\",\"Line\":204,\"confidence\":\"95\",\"HOLD_NUMBER\":10000,\"Tdate\":\"20171128\",\"per\":1},{\"Type\":\"短期融资券\",\"Code\":\"13030700001QKZ\",\"Line\":204,\"confidence\":\"95\",\"HOLD_NUMBER\":10000,\"Tdate\":\"20171128\",\"per\":1},{\"Type\":\"私募债\",\"Code\":\"13030700001LHQ\",\"Line\":204,\"confidence\":\"95\",\"HOLD_NUMBER\":1000,\"Tdate\":\"20171128\",\"per\":1},{\"Type\":\"资产支持证券\",\"Code\":\"0306200003YHZQ\",\"Line\":203,\"confidence\":\"95\",\"HOLD_NUMBER\":10000,\"Tdate\":\"20171128\",\"per\":1},{\"Type\":\"资产支持证券\",\"Code\":\"17015000011ZGZ\",\"Line\":203,\"confidence\":\"95\",\"HOLD_NUMBER\":10000,\"Tdate\":\"20171128\",\"per\":1},{\"Type\":\"非银行金融债\",\"Code\":\"16031500002WDT\",\"Line\":260,\"confidence\":\"95\",\"HOLD_NUMBER\":10000,\"Tdate\":\"20171128\",\"per\":1},{\"Type\":\"非银行金融债\",\"Code\":\"13020500002RZC\",\"Line\":205,\"confidence\":\"95\",\"HOLD_NUMBER\":1000,\"Tdate\":\"20171128\",\"per\":1},{\"Type\":\"商业银行债\",\"Code\":\"13020500002RZC\",\"Line\":205,\"confidence\":\"95\",\"HOLD_NUMBER\":1000,\"Tdate\":\"20171128\",\"per\":1},{\"Type\":\"同业存单\",\"Code\":\"13020500002RZC\",\"Line\":205,\"confidence\":\"95\",\"HOLD_NUMBER\":1000}]";

            
            
            DataTable dt12 = new DataTable();
            dt12 = JsonConvert.DeserializeObject<DataTable>(a12);






            json j = new json();

            //string a = SerializeObject(j.a());
            string a = SerializeObject(j.a1());
            var b = a.ToString();
            //DataSet ds = new DataSet();
            //ds = JsonConvert.DeserializeObject<DataSet>(a);
            DataTable dt = new DataTable();
            dt = JsonConvert.DeserializeObject<DataTable>(a);

            Dictionary<string, Dictionary<string, List<string>>> list = new Dictionary<string, Dictionary<string, List<string>>>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //if (dt.Rows[i][2].ToString() == "244")
                //{
                //    dt.Rows[i][0] = "其他";
                //}
                if (!list.ContainsKey(dt.Rows[i][0].ToString()))
                {
                    list.Add(dt.Rows[i]["Type"].ToString(), new Dictionary<string, List<string>>());
                    list[dt.Rows[i]["Type"].ToString()].Add(dt.Rows[i]["Line"].ToString(), new List<string>());
                    list[dt.Rows[i]["Type"].ToString()][dt.Rows[i]["Line"].ToString()].Add(dt.Rows[i]["Code"].ToString());

                }
                else
                {
                    if (!list[dt.Rows[i][0].ToString()].ContainsKey(dt.Rows[i][2].ToString()))
                    {
                        list[dt.Rows[i][0].ToString()].Add(dt.Rows[i][2].ToString(), new List<string>());
                        list[dt.Rows[i][0].ToString()][dt.Rows[i][2].ToString()].Add(dt.Rows[i][1].ToString());
                    }
                    else
                    {
                        list[dt.Rows[i][0].ToString()][dt.Rows[i][2].ToString()].Add(dt.Rows[i][1].ToString());
                    }
                }
            }




            Person p = new Person();
            p.Age = 1;
            p.Birthday = DateTime.Now;
            p.Name = "2";
            p.Sex = "3";
            string a1 = SerializeObject(p);




        }


        public static bool IsNumber(string s,int precision,int scale)
        {
            if((precision == 0)&&(scale == 0))
            {
                return false;
            }
            string pattern = @"(^\d{1,"+precision+"}";
            if(scale>0)
            {
                pattern += @"\.\d{0,"+scale+"}$)|"+pattern;
            }
            pattern += "$)";
            return Regex.IsMatch(s,pattern);
        }


        public static string SerializeObject(DataTable value)
        {
            string ret = string.Empty;
            try
            {
                ret = JsonConvert.SerializeObject(value);
            }
            catch
            {
                return ret;
            }
            return ret;
        }

        public static string SerializeObject(DataSet value)
        {
            string ret = string.Empty;
            try
            {
                ret = JsonConvert.SerializeObject(value);
            }
            catch
            {
                return ret;
            }
            return ret;
        }


        public static string SerializeObject(Person value)
        {
            string ret = string.Empty;
            try
            {
                ret = JsonConvert.SerializeObject(value);
            }
            catch
            {
                return ret;
            }
            return ret;
        }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class json
    {
        public DataTable a()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Age", Type.GetType("System.Int32"));
            dt.Columns.Add("Name", Type.GetType("System.String"));
            dt.Columns.Add("Sex", Type.GetType("System.String"));
            dt.Columns.Add("IsMarry", Type.GetType("System.Boolean"));
            for (int i = 0; i < 4; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Age"] = i + 1;
                dr["Name"] = "Name" + i;
                dr["Sex"] = i % 2 == 0 ? "男" : "女";
                dr["IsMarry"] = i % 2 > 0 ? true : false;
                dt.Rows.Add(dr);
            }
            return dt;
        }


        public DataTable a1()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Type", Type.GetType("System.String"));
            dt.Columns.Add("Code", Type.GetType("System.String"));
            dt.Columns.Add("Line", Type.GetType("System.Int32"));
            dt.Columns.Add("confidence", Type.GetType("System.String"));
            dt.Columns.Add("HOLD_NUMBER", Type.GetType("System.Int32"));
            dt.Columns.Add("Tdate", Type.GetType("System.String"));
            dt.Columns.Add("per", Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Type"] = "国债";
            dr["Code"] = "IB010011.bond";
            dr["Line"] = 203;
            dr["confidence"] = 95;
            dr["HOLD_NUMBER"] = 1000;
            dr["Tdate"] = "20171128";
            dr["per"] = 1;
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1["Type"] = "短期融资券";
            dr1["Code"] = "IB011751019.bond";
            dr1["Line"] = 260;
            dr1["confidence"] = 95;
            dr1["HOLD_NUMBER"] = 1000;
            dr1["Tdate"] = "20171128";
            dr1["per"] = 1;
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2["Type"] = "私募债";
            dr2["Code"] = "IB031666007.bond";
            dr2["Line"] = 204;
            dr2["confidence"] = 95;
            dr2["HOLD_NUMBER"] = 1000;
            dr2["Tdate"] = "20171128";
            dr2["per"] = 1;
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["Type"] = "资产支持证券";
            dr3["Code"] = "IB061709001.bond";
            dr3["Line"] = 244;
            dr3["confidence"] = 95;
            dr3["HOLD_NUMBER"] = 1000;
            dr3["Tdate"] = "20171128";
            dr3["per"] = 1;
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4["Type"] = "资产支持证券";
            dr4["Code"] = "IB061709002.bond";
            dr4["Line"] = 244;
            dr4["confidence"] = 95;
            dr4["HOLD_NUMBER"] = 1000;
            dr4["Tdate"] = "20171128";
            dr4["per"] = 1;
            dt.Rows.Add(dr4);

            DataRow dr5 = dt.NewRow();
            dr5["Type"] = "非银行金融债";
            dr5["Code"] = "IB065001.bond";
            dr5["Line"] = 275;
            dr5["confidence"] = 95;
            dr5["HOLD_NUMBER"] = 1000;
            dr5["Tdate"] = "20171128";
            dr5["per"] = 1;
            dt.Rows.Add(dr5);

            DataRow dr6 = dt.NewRow();
            dr6["Type"] = "非银行金融债";
            dr6["Code"] = "IB071746003.bond";
            dr6["Line"] = 275;
            dr6["confidence"] = 95;
            dr6["HOLD_NUMBER"] = 1000;
            dr6["Tdate"] = "20171128";
            dr6["per"] = 1;
            dt.Rows.Add(dr6);

            DataRow dr7 = dt.NewRow();
            dr7["Type"] = "商业银行债";
            dr7["Code"] = "IB081102.bond";
            dr7["Line"] = 275;
            dr7["confidence"] = 95;
            dr7["HOLD_NUMBER"] = 1000;
            dr7["Tdate"] = "20171128";
            dr7["per"] = 1;
            dt.Rows.Add(dr7);

            DataRow dr8 = dt.NewRow();
            dr8["Type"] = "同业存单";
            dr8["Code"] = "IB111719381.bond";
            dr8["Line"] = 275;
            dr8["confidence"] = 95;
            dr8["HOLD_NUMBER"] = 1000;
            dr8["Tdate"] = "20171128";
            dr8["per"] = 1;
            dt.Rows.Add(dr8);

            //DataSet ds = new DataSet();
            //ds.Tables.Add(dt);
            //ds.Tables.Add(dt1);

            return dt;
        }


        public string node1 = "1";
        public string node2 = "2";
        public string node3 = "3";
        public string node4 = "4";
    }



    [JsonObject(MemberSerialization.OptOut)]
    public class Person
    {
        public int Age { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }

        [JsonIgnore]
        public bool IsMarry { get; set; }

        public DateTime Birthday { get; set; }
    }
}
