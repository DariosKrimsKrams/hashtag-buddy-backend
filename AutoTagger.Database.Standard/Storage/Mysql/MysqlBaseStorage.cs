namespace AutoTagger.Database.Storage.Mysql
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Database.Storage.Mysql.Generated;
    using Microsoft.EntityFrameworkCore;
    using MySql.Data.MySqlClient;

    public abstract class MysqlBaseStorage
    {
        protected InstataggerContext db;

        protected MysqlBaseStorage()
        {
            db = new InstataggerContext();
        }

        private void Reconnect()
        {
            this.db.Database.CloseConnection();
            this.db.Database.OpenConnection();
        }

        protected void Save()
        {
            try
            {
                this.db.SaveChanges();
            }
            catch (MySqlException e)
            {
                if (e.Message.Contains("Timeout"))
                {
                    this.Reconnect();
                    this.db.SaveChanges();
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
                this.db.Database.OpenConnection();
            }
            catch (MySqlException e)
            {
                if (e.Message.Contains("Timeout"))
                {
                    this.db.Database.OpenConnection();
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        protected IEnumerable<IEnumerable<string>> ExecuteCustomQuery(string query)
        {
            List<string> Func(List<string> entry, string key, string value)
            {
                entry.Add(value);
                return entry;
            }
            return this.ExecuteQuery<List<string>>(query, Func);
        }

        protected IEnumerable<IHumanoidTag> ExecuteHTagsQuery(string query)
        {
            HumanoidTag Func(HumanoidTag entry, string key, string value)
            {
                if (key == "name") entry.Name              = value;
                else if (key == "posts") entry.Posts       = Convert.ToInt32(value);
                else if (key == "id") entry.Id             = Convert.ToInt32(value);
                else if (key == "refCount") entry.RefCount = Convert.ToInt32(value);
                return entry;
            }
            return this.ExecuteQuery<HumanoidTag>(query, Func);
        }

        private IEnumerable<T> ExecuteQuery<T>(string query, Func<T, string, string, T> func) where T : new()
        {
            var result = new List<T>();
            using (var command = this.db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText    = query;
                command.CommandType    = CommandType.Text;
                command.CommandTimeout = 600;

                this.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
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
                this.db.Database.CloseConnection();
            }
            return result;
        }

    }

}
