namespace Instaq.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using Instaq.Common;
    using Instaq.Contract.Models;
    using Instaq.Database.Storage.Mysql.Custom;
    using Instaq.Database.Storage.Mysql.Generated;
    using Microsoft.EntityFrameworkCore;
    using MySql.Data.MySqlClient;

    public abstract class MysqlBaseStorage
    {
        protected InstaqProdContext Db;

        protected MysqlBaseStorage(InstaqProdContext context)
        {
            this.Db = context;
        }

        private void Reconnect()
        {
            this.Db.Database.CloseConnection();
            this.Db.Database.OpenConnection();
        }

        protected void Save()
        {
            try
            {
                this.Db.SaveChanges();
            }
            catch (MySqlException e)
            {
                if (e.Message.Contains("Timeout"))
                {
                    this.Reconnect();
                    this.Db.SaveChanges();
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        protected void OpenConnection()
        {
            try
            {
                this.Db.Database.OpenConnection();
            }
            catch (MySqlException e)
            {
                if (e.Message.Contains("Timeout"))
                {
                    this.Db.Database.OpenConnection();
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        protected (IEnumerable<IEnumerable<string>>, TimeSpan) ExecuteCustomQuery(string query)
        {
            static List<string> Func(List<string> entry, string key, string value)
            {
                entry.Add(value);
                return entry;
            }
            return this.SaveQuery<List<string>>(query, Func);
        }

        protected (IEnumerable<Photos>, TimeSpan) ExecutePhotosQuery(string query)
        {
            static Photos Func(Photos entry, string key, string value)
            {
                switch (key)
                {
                    case "likes":
                        entry.Likes = Convert.ToInt32(value);
                        break;
                    case "comments":
                        entry.Comments = Convert.ToInt32(value);
                        break;
                    case "follower":
                        entry.Follower = Convert.ToInt32(value);
                        break;
                    case "following":
                        entry.Following = Convert.ToInt32(value);
                        break;
                    case "posts":
                        entry.Posts = Convert.ToInt32(value);
                        break;
                }

                return entry;
            }
            return this.SaveQuery<Photos>(query, Func);
        }

        protected (IEnumerable<IHumanoidTag>, TimeSpan) ExecuteHTagsQuery(string query)
        {
            static HumanoidTag Func(HumanoidTag entry, string key, string value)
            {
                switch (key)
                {
                    case "name":
                        entry.Name = value;
                        break;
                    case "posts":
                        entry.Posts = Convert.ToInt32(value);
                        break;
                    case "id":
                        entry.Id = Convert.ToInt32(value);
                        break;
                    case "refCount":
                        entry.RefCount = Convert.ToInt32(value);
                        break;
                }

                return entry;
            }
            return this.SaveQuery<HumanoidTag>(query, Func);
        }

        private (IEnumerable<T>, TimeSpan) SaveQuery<T>(string query, Func<T, string, string, T> func) where T : new()
        {
            try
            {
                return this.ExecuteQuery(query, func);
            }
            catch (MySqlException e)
            {
                if (e.Message.ToLower().Contains("connection"))
                {
                    Console.WriteLine("Error: " + e.Message);
                    Thread.Sleep(3000);
                    this.Reconnect();
                    return this.ExecuteQuery(query, func);
                }
                throw;
            }
        }

        private (IEnumerable<T>, TimeSpan) ExecuteQuery<T>(string query, Func<T, string, string, T> func) where T : new()
        {
            var result = new List<T>();
            TimeSpan time;
            using (var command = this.Db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText    = query;
                command.CommandType    = CommandType.Text;
                command.CommandTimeout = 600;

                this.OpenConnection();
                var startTime = DateTime.Now;
                using (var reader = command.ExecuteReader())
                {
                    var endTime = DateTime.Now;
                    time = endTime - startTime;
                    while (reader.Read())
                    {
                        var entry = new T();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var key   = reader.GetName(i);
                            var value = reader.GetValue(i).ToString();
                            entry = func(entry, key, value);
                        }
                        result.Add(entry);
                    }
                }
                this.Db.Database.CloseConnection();
            }
            return (result, time);
        }

        protected void DetachLocal<T>(T t, int entryId) where T : class, IIdentifier
        {
            var local = this.Db.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(entryId));
            if (local != null)
            {
                this.Db.Entry(local).State = EntityState.Detached;
            }
            this.Db.Entry(t).State = EntityState.Modified;
        }

    }

}
