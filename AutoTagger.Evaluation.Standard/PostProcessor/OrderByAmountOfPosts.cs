using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Evaluation.Standard.PostProcessor
{
    using System.Linq;

    class OrderByAmountOfPosts
    {
        public IEnumerable<IEnumerable<string>> Do(IEnumerable<IEnumerable<string>> input)
        {
            var output = input.ToList();
            output.Sort(delegate (IEnumerable<string> p1, IEnumerable<string> p2)
            {
                var value1 = Convert.ToInt32(p1.LastOrDefault());
                var value2 = Convert.ToInt32(p2.LastOrDefault());
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
