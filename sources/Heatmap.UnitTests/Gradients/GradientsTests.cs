using Heatmap.Gradients;
using Heatmap.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace Heatmap.UnitTests.Gradients
{
    public class GradientsTests
    {
        [Theory]
        [MemberData(nameof(GivenCorrectInputWhenGetColorThenCorrectValueCaseGenerator))]
        public void GivenCorrectInputWhenGetColorThenCorrectValue(IGradient gradient, double position, RgbColor expected)
        {
            // Arrange

            // Act
            var actual = gradient.GetColor(position);

            // Assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GivenCorrectInputWhenGetColorThenCorrectValueCaseGenerator()
        {
            var red = new RgbColor(byte.MaxValue, byte.MinValue, byte.MinValue);
            var blue = new RgbColor(byte.MinValue, byte.MinValue, byte.MaxValue);

            var linear = new LinearGradient(red, blue);
            var positioned = new PositionedGradient(new[] { new PositionedColor(0, red), new PositionedColor(1, blue) });

            // 0.25 position
            {
                var position = 0.25d;
                var expected = new RgbColor(191, 0, 64);
                yield return new object[] { linear, position, expected };
                yield return new object[] { positioned, position, expected };
            }

            // -1 position
            {
                var position = -1d;
                var expected = red;
                yield return new object[] { linear, position, expected };
                yield return new object[] { positioned, position, expected };
            }

            // 2 position
            {
                var position = 2d;
                var expected = blue;
                yield return new object[] { linear, position, expected };
                yield return new object[] { positioned, position, expected };
            }
        }

        [Theory]
        [MemberData(nameof(GivenCorrectPositionToPreCachedGradientWhenGetColorThenSameColorAsParentCaseGenerator))]
        public void GivenCorrectPositionToPreCachedGradientWhenGetColorThenSameColorAsParent(IGradient gradient, double position)
        {
            // Arrange
            var expected = gradient.GetColor(position);
            var preCached = new PreCachedGradient(gradient);

            // Act
            var actual = preCached.GetColor(position);

            // Assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GivenCorrectPositionToPreCachedGradientWhenGetColorThenSameColorAsParentCaseGenerator()
        {
            const int iterations = 256;
            var black = new RgbColor(0, 0, 0);
            var white = new RgbColor(255, 255, 255);

            var linear = new LinearGradient(white, black);
            var positioned = new PositionedGradient(new PositionedColor(0, white), new PositionedColor(1, black));
            
            foreach(var position in Enumerable.Range(0, iterations).Select(iteration => iteration / (double)iterations))
            {
                yield return new object[] { linear, position };
                yield return new object[] { positioned, position };
            }
        }
    }
}
