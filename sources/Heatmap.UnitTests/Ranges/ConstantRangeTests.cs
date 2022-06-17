using Heatmap.Ranges;
using System.Collections.Generic;

namespace Heatmap.UnitTests.Ranges
{
    public class ConstantRangeTests
    {
        [Theory]
        [MemberData(nameof(GivenConstantRangeWhenGetValueThenCorrectValueCaseGenerator))]
        public void GivenConstantRangeWhenGetValueThenCorrectValue(double min, double max, double input, double expected)
        {
            // Arrange
            var range = new ConstantRange(min, max);

            // Act
            var actual = range.GetValue(input);

            // Assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GivenConstantRangeWhenGetValueThenCorrectValueCaseGenerator()
        {
            yield return new object[] { 1d, 2d, 1.5d, 0.5d };
            yield return new object[] { 2d, 5d, 1d, 0d };
            yield return new object[] { 2d, 5d, 6d, 1d };
        }
    }
}
