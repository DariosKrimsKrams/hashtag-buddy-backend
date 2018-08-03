namespace AutoTagger.Contract
{
    using System.Collections.Generic;

    public interface IBlacklistStorage
    {
        void Insert(IEnumerable<string> entries);
    }
}