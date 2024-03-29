﻿namespace Instaq.Database.Storage.Mysql
{
    using System;
    using Instaq.Contract.Models;
    using Instaq.Contract.Storage;
    using Instaq.Database.Storage.Mysql.Generated;

    public class MysqlFeedbackStorage : MysqlBaseStorage, IFeedbackStorage
    {
        public MysqlFeedbackStorage(InstaqContext context)
            : base(context)
        {
        }

        public IFeedback GetLog(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(IFeedback commonFeedback)
        {
            var feedback = LogsFeedback.FromCommonFeedback(commonFeedback);
            this.Db.LogsFeedback.Add(feedback);
            this.Db.SaveChanges();
            return feedback.Id;
        }
    }
}
