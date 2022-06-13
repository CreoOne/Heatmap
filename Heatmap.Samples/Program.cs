using Heatmap.Gradients;
using Heatmap.Primitives;
using Heatmap.Range;
using Heatmap.Samplers;
using Heatmap.SkiaSharp.Receivers;
using System.Numerics;

namespace Heatmap.Samples;

public class Program
{
    public static void Main(string[] args) => MainAsync().Wait();

    private static async Task MainAsync()
    {
        await Ripple();
        await Function();
        await Barcode();
    }

    public async static Task Ripple()
    {
        static double Func(Vector2 position) => Math.Cos((Vector2.Zero - position).Length() * 20d);

        var sampler = new LambdaSampler(Func);
        var receiver = new SkiaSharpReceiver();

        await new DefaultHeatmapBuilder()
            .SetSampler(sampler)
            .SetReceiver(receiver)
            .GenerateAsync();

        await SaveAsync(await receiver.GetPngStreamAsync(new Resolution(400, 400)), nameof(Ripple));
    }

    public async static Task Function()
    {
        static double Func(Vector2 position) => Math.Exp(-Math.Abs(position.X)) * Math.Sin(position.X) * 6f;

        var sampler = new LambdaSampler(Func);
        var receiver = new SkiaSharpReceiver();

        var gradient = new PositionedGradient(new[] {
            new PositionedColor(0, new RgbColor(0, 255, 255)), // cold over-exposure
            new PositionedColor(0.2f, new RgbColor(0, 128, 128)),
            new PositionedColor(0.8f, new RgbColor(128, 0, 0)),
            new PositionedColor(1f, new RgbColor(255, 0, 0)) // hot over-exposure
        });

        await new DefaultHeatmapBuilder()
            .SetSampler(sampler)
            .SetReceiver(receiver)
            .SetViewport(Viewport.FromTwoPoints(new Vector2(-4), new Vector2(4)))
            .SetSamplingResolution(new Resolution(200, 1))
            .SetGradient(gradient)
            .SetRangeFactory(new ConstantRangeFactory(-1.5f, 1.5f)) // cut off
            .GenerateAsync();

        await SaveAsync(await receiver.GetPngStreamAsync(new Resolution(400, 20)), nameof(Function));
    }

    public async static Task Barcode()
    {
        var random = new Random();
        var barcodeData = Enumerable.Range(0, 128).Select(_ => random.Next(0, 2)).ToArray();
        double Func(Vector2 position) => barcodeData[(int)(position.X * barcodeData.Length)];

        var sampler = new LambdaSampler(Func);
        var receiver = new SkiaSharpReceiver();

        await new DefaultHeatmapBuilder()
            .SetSampler(sampler)
            .SetReceiver(receiver)
            .SetViewport(Viewport.FromTwoPoints(new Vector2(0), new Vector2(1)))
            .SetSamplingResolution(new Resolution(barcodeData.Length, 1))
            .GenerateAsync();

        await SaveAsync(await receiver.GetPngStreamAsync(new Resolution(barcodeData.Length * 2, 20)), nameof(Barcode));
    }

    private static async Task SaveAsync(Stream stream, string name)
    {
        using var fileStream = new FileStream($"{name}.png", FileMode.Create);
        await stream.CopyToAsync(fileStream);
    }
}