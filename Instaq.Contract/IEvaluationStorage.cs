﻿namespace Instaq.Contract
{
    using System.Collections.Generic;

    using Instaq.Contract.Models;

    public interface IEvaluationStorage
    {
        (string debug, IEnumerable<IHumanoidTag> htags) FindMostRelevantHumanoidTags(IMachineTag[] mTags);

        (string debug, IEnumerable<IHumanoidTag> htags) FindTrendingHumanoidTags(IMachineTag[] mTags);

        IEnumerable<IEnumerable<string>> GetMtagsWithHighScore();
    }
}