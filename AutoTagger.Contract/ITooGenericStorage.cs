using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Contract
{
    public interface ITooGenericStorage
    {
        IEnumerable<IHumanoidTag> GetHumanoidTags(int count, int lastId = 0);
    }
}
