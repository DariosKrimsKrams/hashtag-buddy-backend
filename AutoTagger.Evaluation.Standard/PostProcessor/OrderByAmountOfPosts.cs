using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Evaluation.Standard.PostProcessor
{
    using System.Linq;
    using AutoTagger.Contract;

    class OrderByAmountOfPosts
    {
        public IEnumerable<IHumanoidTag> Do(IEnumerable<IHumanoidTag> htags)
        {
            var output = htags.ToList();
            output.Sort(delegate (IHumanoidTag p1, IHumanoidTag p2)
            {
                var value1 = Convert.ToInt32(p1.Posts);
                var value2 = Convert.ToInt32(p2.Posts);
                if (value1 > value2)
                    return -1;
                if (value1 < value2)
                    return 1;
                return 0;
            });
            return output;
        }
    }
}
