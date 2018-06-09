using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Contract
{
    public interface IEvaluation
    {
        IEnumerable<string> GetHumanoidTags(IAutoTaggerStorage storage, IEnumerable<IMTag> mTags);
    }
}
