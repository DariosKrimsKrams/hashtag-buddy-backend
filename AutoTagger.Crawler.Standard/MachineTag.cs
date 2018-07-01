using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Crawler.Standard
{
    using AutoTagger.Contract;

    public class MachineTag : IMachineTag
    {
        public string Name { get; set; }

        public float Score { get; set; }

        public string Source { get; set; }

        public MachineTag()
        {

        }

        public MachineTag(string name, float score, string source)
        {
            this.Name = name;
            this.Score = score;
            this.Source = source;
        }
    }
}
