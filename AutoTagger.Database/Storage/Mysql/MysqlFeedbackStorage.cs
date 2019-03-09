using AutoTagger.Contract;
using AutoTagger.Contract.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Database.Storage.Mysql
{
    public class MysqlFeedbackStorage : MysqlBaseStorage, IFeedbackStorage
    {
        public IFeedback GetLog(int id)
        {
            throw new NotImplementedException();
        }

        public int InsertLog(IFeedback feedback)
        {
            throw new NotImplementedException();
        }
    }
}
