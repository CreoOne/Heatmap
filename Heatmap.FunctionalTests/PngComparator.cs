using SkiaSharp;
using System.Runtime.CompilerServices;
using Xunit.Sdk;

namespace Heatmap.FunctionalTests
{
    internal static class PngComparator
    {
        const byte ImportanceMask = 0xfc;

        public static void Equal(string patternFileName, Stream actual)
        {
            var patternName = Path.GetFileNameWithoutExtension(patternFileName);
            using var patternStream = new FileStream(patternFileName, FileMode.Open);

            if (!EqualInternal(patternStream, actual, patternName).Success)
                throw new XunitException();
        }

        internal static ComparisonResult EqualInternal(Stream expected, Stream actual, string caseName)
        {
            using var expectedBitmap = SKBitmap.Decode(expected);
            using var actualBitmap = SKBitmap.Decode(actual);

            if (expectedBitmap.Width != actualBitmap.Width)
                return new ComparisonResult { Success = false };

            if(expectedBitmap.Height != actualBitmap.Height)
                return new ComparisonResult { Success = false };

            using var differenceBitmap = new SKBitmap(expectedBitmap.Width, expectedBitmap.Height);
            var differences = 0;

            for (int y = 0; y < expectedBitmap.Height; y++)
                for (int x = 0; x < expectedBitmap.Width; x++)
                {
                    var expectedPixel = expectedBitmap.GetPixel(x, y);
                    var actualPixel = actualBitmap.GetPixel(x, y);
                    var difference = Diff(expectedPixel, actualPixel, ref differences);
                    differenceBitmap.SetPixel(x, y, difference);
                }

            if (differences > 0)
            {
                var comparisonResult = new ComparisonResult
                {
                    Success = false,
                    ActualFilePath = GetFileName(caseName, "actual"),
                    ExpectedFilePath = GetFileName(caseName, "expected"),
                    DifferenceFilePath = GetFileName(caseName, "difference")
                };

                Save(differenceBitmap, comparisonResult.DifferenceFilePath);
                Save(actualBitmap, comparisonResult.ActualFilePath);
                Save(expectedBitmap, comparisonResult.ExpectedFilePath);

                var pixels = expectedBitmap.Width * expectedBitmap.Height;
                var percentage = differences / (double)pixels;
                //throw new XunitException($"Images differ in {differences}/{pixels} pixels ({percentage:0.00%})");
                return comparisonResult;
            }

            return new ComparisonResult { Success = true };
        }

        private static string GetFileName(string caseName, string suffix) => string.Join('.', caseName, suffix, "png");

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

            if(colorDifference)
                differenceCounter++;

            return new SKColor(colorDifference ? byte.MaxValue : byte.MinValue, 0, alphaDifference ? byte.MaxValue : byte.MinValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Diff(byte expected, byte actual) => (expected & ImportanceMask) != (actual & ImportanceMask);
    }
}
