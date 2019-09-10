using Instaq.Contract;
using Instaq.Contract.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Instaq.Database.Storage.Mysql
{
    using System.Linq;

    public class MysqlFeedbackStorage : MysqlBaseStorage, IFeedbackStorage
    {

        public int Insert(IFeedback commonFeedback)
        {
            var feedback = Feedback.FromCommonFeedback(commonFeedback);
            this.db.Feedback.Add(feedback);
            this.db.SaveChanges();
            return feedback.Id;
        }

        public IFeedback GetLog(int id)
        {
            throw new NotImplementedException();
        }
    }
}
