using Xunit.Abstractions;

namespace Heatmap.FunctionalTests
{
    public static class AssertPng
    {
        public static void Equal(string patternFileName, Stream actual, ITestOutputHelper testOutputHelper)
        {
            var patternName = Path.GetFileNameWithoutExtension(patternFileName);
            using var patternStream = new FileStream(patternFileName, FileMode.Open);

            var result = PngComparator.Equal(patternStream, actual, patternName);

            foreach (var message in result.ErrorMessages)
                testOutputHelper.WriteLine(message);

            Assert.True(result.Success);
        }
    }
}
