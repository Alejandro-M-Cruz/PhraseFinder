﻿using PhraseFinder.Domain.Models;

namespace PhraseFinder.Domain.Services.PatternGenerators;

public interface IPatternGenerator
{
    public IEnumerable<Phrase> GeneratePatterns(Phrase phrases);
}
