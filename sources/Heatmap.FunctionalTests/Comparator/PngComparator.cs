using Heatmap.FunctionalTests.Primitives;
using SkiaSharp;
using System.Runtime.CompilerServices;

namespace Heatmap.FunctionalTests.Comparator
{
    internal static class PngComparator
    {
        const byte ImportanceMask = 0xfc;
        const string TestArtifactsDirectoryName = "FunctionalTestsFailsArtifacts";

        internal static ComparisonResult Equal(Stream expected, Stream actual, string caseName)
        {
            var errorMessages = new List<string>();
            using var expectedBitmap = SKBitmap.Decode(expected);
            using var actualBitmap = SKBitmap.Decode(actual);

            if (expectedBitmap.Width != actualBitmap.Width)
                errorMessages.Add($"Size differs in width. Expected {expectedBitmap.Width}. Actual {actualBitmap.Width}.");

            if (expectedBitmap.Height != actualBitmap.Height)
                errorMessages.Add($"Size differs in height. Expected {expectedBitmap.Height}. Actual {actualBitmap.Height}.");

            var width = Math.Min(expectedBitmap.Width, actualBitmap.Width);
            var height = Math.Min(expectedBitmap.Height, actualBitmap.Height);

            using var differenceBitmap = new SKBitmap(width, height);
            var differences = 0;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    var expectedPixel = expectedBitmap.GetPixel(x, y);
                    var actualPixel = actualBitmap.GetPixel(x, y);
                    var difference = Diff(expectedPixel, actualPixel, ref differences);
                    differenceBitmap.SetPixel(x, y, difference);
                }

            if (differences > 0)
            {
                var pixels = width * height;
                var percentage = differences / (double)pixels;
                errorMessages.Add($"Images differ in {differences}/{pixels} pixels ({percentage:0.00%}). Includes only pixels that are within both images sizes.");

                var comparisonResult = new ComparisonResult
                {
                    Success = false,
                    ActualFilePath = GetFileName(caseName, "actual"),
                    ExpectedFilePath = GetFileName(caseName, "expected"),
                    DifferenceFilePath = GetFileName(caseName, "difference"),
                    ErrorMessages = errorMessages
                };

                Directory.CreateDirectory(TestArtifactsDirectoryName);
                Save(differenceBitmap, comparisonResult.DifferenceFilePath);
                Save(actualBitmap, comparisonResult.ActualFilePath);
                Save(expectedBitmap, comparisonResult.ExpectedFilePath);

                return comparisonResult;
            }

            return new ComparisonResult
            {
                Success = !errorMessages.Any(),
                ErrorMessages = errorMessages
            };
        }

        private static string GetFileName(string caseName, string suffix) => Path.Combine(TestArtifactsDirectoryName, string.Join('.', caseName, suffix, "png"));

        private static void Save(SKBitmap bitmap, string fileName)
        {
            using var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = new FileStream(fileName, FileMode.Create);
            data.SaveTo(stream);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static SKColor Diff(SKColor expected, SKColor actual, ref int differenceCounter)
        {
            var redDifference = Diff(expected.Red, actual.Red);
            var greenDifference = Diff(expected.Green, actual.Green);
            var blueDifference = Diff(expected.Blue, actual.Blue);
            var alphaDifference = Diff(expected.Alpha, actual.Alpha);

            var colorDifference = redDifference || greenDifference || blueDifference;

            if (colorDifference)
                differenceCounter++;

            return new SKColor(colorDifference ? byte.MaxValue : byte.MinValue, 0, alphaDifference ? byte.MaxValue : byte.MinValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Diff(byte expected, byte actual) => (expected & ImportanceMask) != (actual & ImportanceMask);
    }
}
