using Heatmap.Primitives;
using System.Numerics;

namespace Heatmap.UnitTests.Primitives
{
    public sealed class ViewportTests
    {
        [Fact]
        public void GivenCorrectInputDataWhenRetrievingFromThenCorrectValue()
        {
            // Arrange
            var viewport = new Viewport(new Vector2(4f, 1f), new Vector2(2f, 3f));

            // Act
            var result = viewport.From;

            // Assert
            Assert.Equal(new Vector2(2f, 1f), result);
        }

        [Fact]
        public void GivenCorrectInputDataWhenRetrievingToThenCorrectValue()
        {
            // Arrange
            var viewport = new Viewport(new Vector2(4f, 1f), new Vector2(2f, 3f));

            // Act
            var result = viewport.To;

            // Assert
            Assert.Equal(new Vector2(4f, 3f), result);
        }

        [Fact]
        public void GivenCorrectInputDataWhenGetViewPointThenCorrectValue()
        {
            // Arrange
            var viewport = new Viewport(new Vector2(2f), new Vector2(3f));

            // Act
            var actual = viewport.GetViewPoint(new Vector2(0.25f, 0.75f));

            // Assert
            Assert.Equal(new Vector2(2.25f, 2.75f), actual);
        }
    }
}
