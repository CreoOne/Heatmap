using Xunit;
using Heatmap.Primitives;
using System.Numerics;

namespace Heatmap.UnitTests.Primitives
{
    public sealed class SampleTests
    {
        [Fact]
        public void GivenCorrectInputWhenRetrievingPositionThenCorrectValue()
        {
            // Arrange
            var sample = new Sample(new Vector2(2f, 3f), 4f);

            // Act
            var result = sample.Position;

            // Assert
            Assert.Equal(new Vector2(2f, 3f), result);
        }

        [Fact]
        public void GivenCorrectInputWhenRetrievingValueThenCorrectValue()
        {
            // Arrange
            var sample = new Sample(new Vector2(2f, 3f), 4f);

            // Act
            var result = sample.Value;

            // Assert
            Assert.Equal(4f, result);
        }
    }
}
