namespace Instaq.Contract.Dto
{
    using System;
    using System.Collections.Generic;
    using Instaq.Contract.Models;

    public interface IEvaluationDto
    {
        string Query { get; set; }

        IEnumerable<IHumanoidTag> HumanoidTags { get; set; }

        TimeSpan TimeNeeded { get; set; }

    }
}
