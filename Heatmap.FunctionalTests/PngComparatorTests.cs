namespace Heatmap.FunctionalTests
{
    public class PngComparatorTests
    {
        [Fact]
        public void GivenTwoImagesWhenEqualCorrectOutput()
        {
            // Arrange
            using var qImage = new FileStream("Images/Q.png", FileMode.Open);
            using var rImage = new FileStream("Images/R.png", FileMode.Open);

            // Act
            var comparisonResult = PngComparator.EqualInternal(qImage, rImage, nameof(GivenTwoImagesWhenEqualCorrectOutput));

            // Assert
            using var actual = new FileStream(comparisonResult.DifferenceFilePath, FileMode.Open);
            PngComparator.Equal("Images/Difference.png", actual);

            // Cleanup only on success by design
            File.Delete(comparisonResult.ActualFilePath);
            File.Delete(comparisonResult.ExpectedFilePath);
            File.Delete(comparisonResult.DifferenceFilePath);
        }
    }
}