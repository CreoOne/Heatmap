using Heatmap.Gradients;
using Heatmap.Primitives;
using System.Collections.Generic;
using Xunit;

namespace Heatmap.UnitTests.Gradients
{
    public class GradientsTests
    {
        [Theory]
        [MemberData(nameof(GivenCorrectInputWhenGetColorThenCorrectValueCaseGenerator))]
        public void GivenCorrectInputWhenGetColorThenCorrectValue(IGradient gradient, float position, RgbColor expected)
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
                var position = 0.25f;
                var expected = new RgbColor(191, 0, 64);
                yield return new object[] { linear, position, expected };
                yield return new object[] { positioned, position, expected };
            }

            // -1 position
            {
                var position = -1f;
                var expected = red;
                yield return new object[] { linear, position, expected };
                yield return new object[] { positioned, position, expected };
            }

            // 2 position
            {
                var position = 2f;
                var expected = blue;
                yield return new object[] { linear, position, expected };
                yield return new object[] { positioned, position, expected };
            }
        }
    }
}
