namespace Instaq.Common.Dto
{
    using System;
    using System.Collections.Generic;
    using Instaq.Contract.Dto;
    using Instaq.Contract.Models;

    public class EvaluationDto : IEvaluationDto
    {
        public string Query { get; set; }

        public IEnumerable<IHumanoidTag> HumanoidTags { get; set; }

        public TimeSpan TimeNeeded { get; set; }
    }
}
