using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

var logger = new BenchmarkDotNet.Loggers.ConsoleLogger();
var config = ManualConfig.Create(DefaultConfig.Instance)
    .AddLogger(logger);

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);