using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSon
{
    public class JsonToObject
    {
        /// <summary>
        /// 将复杂的Json转换成对象
        /// </summary>
        public static void JsonToObjectFun()
        {
            string json = ObjectToJson();
            json = "{ \"_id\" : 10014388, \"tcr0055_v3\" : [{ \"cr0055_001\" : 10014388, \"id\" : 8652537 }] }";
            MongoJsonConvertToObject obj = JsonConvert.DeserializeObject<MongoJsonConvertToObject>(json);
            int _id = JsonConvert.DeserializeObject<MongoJsonConvertToObject>(json)._id;
            int id = JsonConvert.DeserializeObject<MongoJsonConvertToObject>(json).tcr0055_v3[0].id;
            string cr0055_001 = JsonConvert.DeserializeObject<MongoJsonConvertToObject>(json).tcr0055_v3[0].cr0055_001;
        }

        /// <summary>
        /// 将复杂的对象转换成Json
        /// </summary>
        /// <returns></returns>
        public static string ObjectToJson()
        {
            List<ObjectItem> ll = new List<ObjectItem>();
            ObjectItem i = new ObjectItem();
            i.id = 11;
            i.cr0055_001 = "22";
            ll.Add(i);
            var w = new MongoJsonConvertToObject
            {
                _id = 1,
                tcr0055_v3 = ll
            };
            string json = JsonConvert.SerializeObject(w);
            return json;
        }
    }

    /// <summary>
    /// 将MongoDB中提取的数据转换成对象
    /// </summary>
    public class MongoJsonConvertToObject
    {
        public int _id { get; set; }
        /// <summary>
        /// 子对像列表
        /// </summary>
        public List<ObjectItem> tcr0055_v3 = new List<ObjectItem>();
    }

    /// <summary>
    /// 子对象
    /// </summary>
    public class ObjectItem
    {
        public int id { get; set; }
        public string cr0055_001 { get; set; }
    }
}
