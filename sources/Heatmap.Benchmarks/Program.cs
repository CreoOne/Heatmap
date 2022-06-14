// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

var config = DefaultConfig
    .Instance
    .AddDiagnoser(MemoryDiagnoser.Default);

BenchmarkRunner.Run(typeof(Program).Assembly, config);