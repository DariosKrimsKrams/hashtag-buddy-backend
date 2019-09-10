namespace Instaq.Contract.Models
{
    using System;
    using System.Collections.Generic;

    using Instaq.Contract.Models;

    public interface IBlacklistStorage
    {
        void Insert(IList<IBlacklistEntry> entries);

        void Delete(string reason, string table);

        (IEnumerable<ITag> tags, TimeSpan time) GetTags(string tableName, int limit = 1000);

        //void UpdateTags(IEnumerable<string> tags, string table);

        void UpdateTags(ITag[] tags, string table);

    }
}