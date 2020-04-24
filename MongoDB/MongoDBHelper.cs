using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MongoDB
{
    class MongoDBHelper<T>
    {
        private IMongoCollection<T> collection = null;
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public bool Update(Expression<Func<T, bool>> express, T entity)
        {
            try
            {
                var old = collection.Find(express).ToList().FirstOrDefault();

                foreach (var prop in entity.GetType().GetProperties())
                {
                    if (!prop.Name.Equals("_id"))
                    {
                        var newValue = prop.GetValue(entity);
                        var oldValue = old.GetType().GetProperty(prop.Name).GetValue(old);

                        if (newValue != null)
                        {
                            if (oldValue == null)
                                oldValue = "";
                            if (!newValue.ToString().Equals(oldValue.ToString()))
                            {
                                old.GetType().GetProperty(prop.Name).SetValue(old, newValue.ToString());
                            }
                        }
                    }
                }

                // var filter = Builders<T>.Filter.Eq("Id", entity.Id);
                ReplaceOneResult result = collection.ReplaceOneAsync(express, old).Result;
                if (result.ModifiedCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                var aaa = ex.Message + ex.StackTrace;
                throw;
            }
        }
    }
}
