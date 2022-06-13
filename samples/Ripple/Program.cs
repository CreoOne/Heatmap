using Heatmap;
using Heatmap.Primitives;
using Heatmap.Samplers;
using Heatmap.SkiaSharp.Receivers;
using System.Numerics;

static double Func(Vector2 position) => Math.Cos((Vector2.Zero - position).Length() * 20d);

var sampler = new LambdaSampler(Func);
var receiver = new SkiaSharpReceiver();

await new DefaultHeatmapBuilder()
    .SetSampler(sampler)
    .SetReceiver(receiver)
    .GenerateAsync();

var fileName = $"{typeof(Program).Namespace}.png";
var pngStream = await receiver.GetPngStreamAsync(new Resolution(400, 400));
using var fileStream = new FileStream(fileName, FileMode.Create);
await pngStream.CopyToAsync(fileStream);