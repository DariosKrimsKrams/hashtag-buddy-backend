namespace AutoTagger.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
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

        // TODO refactoring

        protected IEnumerable<IEnumerable<string>> ExecuteCustomQuery(string query)
        {
            var entries = new List<IEnumerable<string>>();
            using (var command = this.db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                this.db.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var entry = new List<string>();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            entry.Add(reader.GetValue(i).ToString());
                        }
                        entries.Add(entry);
                    }
                }
                this.db.Database.CloseConnection();
            }
            return entries;
        }

        protected IEnumerable<IHumanoidTag> ExecuteHTagsQuery(string query)
        {
            var entries = new List<IHumanoidTag>();
            using (var command = this.db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                this.db.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var htag = new HumanoidTag();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var value = reader.GetValue(i).ToString();
                            // TODO use keys instead of i
                            if (i == 0)
                                htag.Name = value;
                            else if(i == 1)
                                htag.Posts = Convert.ToInt32(value);
                            else if(i == 2)
                                htag.Id = Convert.ToInt32(value);
                        }
                        entries.Add(htag);
                    }
                }
                this.db.Database.CloseConnection();
            }
            return entries;
        }

    }

}
