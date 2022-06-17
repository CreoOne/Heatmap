using Heatmap;
using Heatmap.Gradients;
using Heatmap.Primitives;
using Heatmap.Ranges;
using Heatmap.Samplers;
using Heatmap.SkiaSharp.Receivers;
using System.Numerics;

static double Func(Vector2 position) => Math.Exp(-Math.Abs(position.X)) * Math.Sin(position.X) * 6f;

var sampler = new LambdaSampler(Func);
var receiver = new SkiaSharpReceiver();

var gradient = new PositionedGradient(new[]
{
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

var fileName = $"{typeof(Program).Namespace}.png";
var pngStream = await receiver.GetPngStreamAsync(new Resolution(400, 20));
using var fileStream = new FileStream(fileName, FileMode.Create);
await pngStream.CopyToAsync(fileStream);
