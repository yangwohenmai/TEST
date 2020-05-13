using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDB
{
    class Program
    {
        static IMongoDatabase Database = new MongoClient("mongodb://finchina:finchina@10.15.97.183:27017/DerivedData").GetDatabase("DerivedData");
        static void Main(string[] args)
        {
            updata();
            Console.WriteLine("Hello World!");
            List<WriteModel<BsonDocument>> InputDBList = new List<WriteModel<BsonDocument>>();
            IMongoCollection<BsonDocument> hk_test = Database.GetCollection<BsonDocument>("test");

            


            var projection = Builders<BsonDocument>.Projection.Include("ITCode2").Exclude("_id");
            //var projection = Builders<BsonDocument>.Projection.Include("ID").Include("ITCode2").Include("TMSTAMP").Exclude("_id");
            var filter1 = Builders<BsonDocument>.Filter.Ne("ID", 11);
            var result = Database.GetCollection<BsonDocument>("hk_test").Find("{}").Project(projection).ToList().ToJson();
            var Fin_List = BsonSerializer.Deserialize<List<teststru>>(result).AsQueryable().ToList();
            var Fin_List1 = BsonSerializer.Deserialize<List<string>>(result).AsQueryable().Distinct().ToList();

            var rep = new ReplaceOneModel<BsonDocument>(filter1, new BsonDocument("ID", 14).Add("ITCode2", "ok").Add("TMSTAMP","OK"));
            rep.IsUpsert = true;
            InputDBList.Add(rep);
            hk_test.BulkWrite(InputDBList);





            var filter = Builders<BsonDocument>.Filter.Eq("ID", 11);
            //var update = Builders<BsonDocument>.Update.Set("ITCode2", "OK2").Set("TMSTAMP", "OK2");
            var update = Builders<BsonDocument>.Update.Set("ITCode2", new BsonDateTime(DateTime.Now));
            Database.GetCollection<BsonDocument>("test").UpdateMany("{}", update);
            //Database.GetCollection<BsonDocument>("hk_test").UpdateMany(filter, update, new UpdateOptions { IsUpsert = true});
            //var updateOptions = new UpdateOptions { IsUpsert = true };
            //InputDBList.Add(new UpdateManyModel<BsonDocument>(filter, update));
            var a = new UpdateManyModel<BsonDocument>(filter, update);
            a.IsUpsert = true;
            InputDBList.Add(a);
            hk_test.BulkWrite(InputDBList);


            //var projection = Builders<BsonDocument>.Projection.Include("ID").Include("ITCode2").Include("TMSTAMP").Exclude("_id");
            //var result = Database.GetCollection<BsonDocument>("hk_test").Find(new BsonDocument()).Project(projection).ToList().ToJson();
            ////var document2 = Database.GetCollection<BsonDocument>("TCR0002_V2").Find(new BsonDocument()).Project(projection).ToList().ToJson();
            //var DataList = BsonSerializer.Deserialize<List<BsonDocument>>(result).AsQueryable().ToList();
        }

        public static  void updata()
        {
            List<WriteModel<BsonDocument>> InputDBList = new List<WriteModel<BsonDocument>>();
            IMongoDatabase Database = new MongoClient("mongodb://finchina:finchina@10.15.97.183:27017/DerivedData").GetDatabase("DerivedData");
            IMongoCollection<BsonDocument> CompanyList_ITCode2_Input = Database.GetCollection<BsonDocument>("test");
            var filter = Builders<BsonDocument>.Filter.Eq("ITCode2", "1");
            var update = Builders<BsonDocument>.Update.Set("ENTRYDT", new BsonDateTime(DateTime.Now.AddHours(8)));
            var upsert = new UpdateManyModel<BsonDocument>(filter, update);
            upsert.IsUpsert = true;
            InputDBList.Add(upsert);
            CompanyList_ITCode2_Input.BulkWrite(InputDBList);
        }
    }

    


    public class teststru
    {
        public int ID;
        public string ITCode2;
        public string TMSTAMP;
    }
}
