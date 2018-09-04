namespace AutoTagger.Crawler.V3.Queue
{
    using System;
    using System.Collections.Generic;

    using AutoTagger.Contract;

    class UserQueue<T> : BaseQueue<T>
    {
        public new void Process(Action<T> userPageCrawling)
        {
            while (this.GetEntry(out T entry))
            {
                //this.AddProcessed(entry);

                var userName = (T) Convert.ChangeType(entry, typeof(T));
                userPageCrawling(userName);
            }
        }
    }
}
