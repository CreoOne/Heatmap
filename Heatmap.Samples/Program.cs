using Heatmap.Gradients;
using Heatmap.Primitives;
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
        await Rastrigin();
    }

    public async static Task Ripple()
    {
        static float Func(Vector2 position) => MathF.Cos((Vector2.Zero - position).Length() * 20f);

        var sampler = new LambdaSampler(Func);
        var receiver = new SkiaSharpReceiver();

        await new DefaultHeatmapBuilder()
            .SetSampler(sampler)
            .SetReceiver(receiver)
            .GenerateAsync();

        await SaveAsync(await receiver.GetPngStreamAsync(400, 400), nameof(Ripple));
    }

    /// <summary>
    /// https://en.wikipedia.org/wiki/Rastrigin_function
    /// </summary>
    public async static Task Rastrigin()
    {
        static float Func(Vector2 position) => 10 * 2 + MathF.Pow(position.X, 2) + MathF.Pow(position.Y, 2) - 10 * MathF.Cos(2 * MathF.PI * position.X) - 10 * MathF.Cos(2 * MathF.PI * position.Y);

        var sampler = new LambdaSampler(Func);
        var receiver = new SkiaSharpReceiver();

        // Viridis
        // https://bids.berkeley.edu/events/better-default-colormap-matplotlib
        var gradient = new LinearGradient(
            new RgbColor(68, 1, 84),
            new RgbColor(65, 68, 135),
            new RgbColor(42, 120, 142),
            new RgbColor(34, 168, 132),
            new RgbColor(122, 209, 81),
            new RgbColor(253, 231, 37)
        );

        await new DefaultHeatmapBuilder()
            .SetSampler(sampler)
            .SetReceiver(receiver)
            .SetViewport(new Viewport(new Vector2(-5.12f), new Vector2(5.12f)))
            .SetSamplingResolution(new Vector2(400, 400))
            .SetGradient(gradient)
            .GenerateAsync();

        await SaveAsync(await receiver.GetPngStreamAsync(400, 400), nameof(Rastrigin));
    }

    private static async Task SaveAsync(Stream stream, string name)
    {
        using var fileStream = new FileStream($"{name}.png", FileMode.Create);
        await stream.CopyToAsync(fileStream);
    }
}