namespace AutoTagger.Database.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using global::AutoTagger.Database.Mysql;
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

        protected IEnumerable<string> ExecuteCustomQuery(string query)
        {
            var entries = new List<string>();
            using (var command = this.db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                this.db.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var htag = reader.GetValue(0).ToString();
                        entries.Add(htag);
                    }
                }
                this.db.Database.CloseConnection();
            }
            return entries;
        }
    }

}
