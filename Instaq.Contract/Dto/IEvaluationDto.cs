namespace Instaq.Contract.Dto
{
    using System.Collections.Generic;
    using Instaq.Contract.Models;

    public interface IEvaluationDto
    {
        string Query { get; set; }

        IEnumerable<IHumanoidTag> HumanoidTags { get; set; }

        double TimeNeeded { get; set; }

    }
}
