using Heatmap.Ranges;
using System.Collections.Generic;

namespace Heatmap.UnitTests.Ranges
{
    public class AdaptiveRangeTests
    {
        [Theory]
        [MemberData(nameof(GivenConstantRangeWhenGetValueThenCorrectValueCaseGenerator))]
        public void GivenAdaptiveRangeWhenGetValueThenCorrectValue(IEnumerable<double> preRanged, double input, double expected)
        {
            // Arrange
            var range = new AdaptiveRange(preRanged);

            // Act
            var actual = range.GetValue(input);

            // Assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GivenConstantRangeWhenGetValueThenCorrectValueCaseGenerator()
        {
            yield return new object[] { new[] { 2d, 1.5d, 1d, 2d }, 1.5d, 0.5d };
            yield return new object[] { new[] { 4d, 2.3d, 2d, 5d }, 1d, 0d };
            yield return new object[] { new[] { 4d, 3d, 2.6d, 5d }, 6d, 1d };
        }
    }
}
