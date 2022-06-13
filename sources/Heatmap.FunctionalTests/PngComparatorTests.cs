using Xunit.Abstractions;

namespace Heatmap.FunctionalTests
{
    public class PngComparatorTests
    {
        public ITestOutputHelper TestOutputHelper { get; }

        public PngComparatorTests(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GivenTwoImagesWhenEqualCorrectOutput()
        {
            // Arrange
            using var qImage = new FileStream("Images/Q.png", FileMode.Open);
            using var rImage = new FileStream("Images/R.png", FileMode.Open);

            // Act
            var comparisonResult = PngComparator.Equal(qImage, rImage, nameof(GivenTwoImagesWhenEqualCorrectOutput));

            // Assert
            using var actual = new FileStream(comparisonResult.DifferenceFilePath, FileMode.Open);
            AssertPng.Equal("Images/Difference.png", actual, TestOutputHelper);

            // Cleanup only on success by design
            File.Delete(comparisonResult.ActualFilePath);
            File.Delete(comparisonResult.ExpectedFilePath);
            File.Delete(comparisonResult.DifferenceFilePath);
        }
    }
}