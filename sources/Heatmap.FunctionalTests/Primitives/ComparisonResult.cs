﻿namespace Heatmap.FunctionalTests.Primitives
{
    public readonly struct ComparisonResult
    {
        public bool Success { get; init; }
        public string ActualFilePath { get; init; }
        public string DifferenceFilePath { get; init; }
        public string ExpectedFilePath { get; init; }
        public IEnumerable<string> ErrorMessages { get; init; }
    }
}
