﻿namespace Instaq.Database.Storage.Mysql
{
    using System;

    using Instaq.Contract;
    using Instaq.Contract.Models;
    using Instaq.Database.Storage.Mysql.Generated;

    using Feedback = Instaq.Database.Feedback;

    public class MysqlFeedbackStorage : MysqlBaseStorage, IFeedbackStorage
    {
        public MysqlFeedbackStorage(InstaqProdContext context)
            : base(context)
        {
        }

        public IFeedback GetLog(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(IFeedback commonFeedback)
        {
            var feedback = Feedback.FromCommonFeedback(commonFeedback);
            this.db.Feedback.Add(feedback);
            this.db.SaveChanges();
            return feedback.Id;
        }
    }
}
