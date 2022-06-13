# How to work with Heatmap

## Data processing pipeline

Data is processed in order described below.

1. Sampling
2. Ranging
3. Color mapping
4. Receiving

## 1. Sampling

In this step data is retrieved, using `Sampler`, from data source in `processor calls for data` manner, meaning that during sampling, library will ask for value specific to particular `X` and `Y` position and expects to receive value.

Currently sampling axes are fixed in directions, `X` coordinate is smaller on the left and bigger on the right, while `Y` coordinate is smaller on top and bigger on the bottom. For situations when axes differ from what described above it becomes programmer responsibility to translate it correctly.

### Synchronous sampling

Currently only synchronous sampling is supported. Order of samples requested may be random in relation to axis coordinates. Next sample is not requested until previous one finished.

### Viewport

Setting viewport allows to explicitly set area of information for processing. It is always axis-aligned rectangle. By default starts from point `(-1, -1)` and ends in `(1, 1)`.

`Sampler` must be written in a way that returns correct information for data that is outside viewport.

_Example using IHeatmapBuilder_
```csharp
var min = new Vector2(-5f, -5f);
var max = new Vector2(5f, 10f);
var viewport = Viewport.FromTwoPoints(min, max);
heatmapBuilder.SetViewport(viewport);
```

### Sampling resolution

Sampling resolution describes amount of samples gathered in relation to `X` and `Y` axis.

_Example using IHeatmapBuilder_
```csharp
var resolution = new Resolution(100, 50);
heatmapBuilder.SetSamplingResolution(resolution);
```

## 2. Ranging

Value of particular sample needs to be normalized between maximal and minimal bounds. This happens inside `Range` step. All ranges are currently linear in nature.

### Constant range

Sets specific minima and maxima to be used. Samples out of range are automatically clamped to be within specified range.

Note that, there is no way to present over-exposed and under-exposed values.

_Example using IHeatmapBuilder_
```csharp
var rangeFactory = new ConstantRangeFactory(-1f, 1f);
heatmapBuilder.SetRangeFactory(rangeFactory);
```

### Adaptive range

Sets minima and maxima from all samples that got gathered. This can result in details missing when very small and very large samples are present within same dataset.

_Example using IHeatmapBuilder_
```csharp
var rangeFactory = new AdaptiveRangeFactory();
heatmapBuilder.SetRangeFactory(rangeFactory);
```

## 3. Color mapping

All ranged samples now become `fragments` and get assigned color that is calculated based on position between `0` and `1` using `gradient`. Where `0` is minima and `1` is maxima specified in ranging step.

Currently no predefined color `gradients` are present in repository and need to be specified manually every time.

### Linear gradient

Uses constant linear distance between list of `RGB` colors to create `gradient`.

_Example using IHeatmapBuilder_
```csharp
var black = new RgbColor(0, 0, 0);
var white = new RgbColor(255, 255, 255);
var gradient = new LinearGradient(white, black); // black-hot
heatmapBuilder.SetGradient(gradient);
```

### Positioned color gradient

Uses linearly described position for colors allowing for non-uniform distances between colors.

When `gradient` is smaller than `0` to `1` range extreme colors get stretched without changing described colors (No interpolation is done, just closest color used outside gradient bounds).

_Example using IHeatmapBuilder_
```csharp
var black = new PositionedColor(0.25f, new RgbColor(0, 0, 0));
var white = new PositionedColor(0.75f, new RgbColor(255, 255, 255));
var gradient = new PositionedGradient(white, black); // black-hot
heatmapBuilder.SetGradient(gradient);
```

## 4. Receiving

Anything can be `receiver`. It receives `fragment` which contains color, position and size information to generate end artifact, most likely image in specific format (like png for example).