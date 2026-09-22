using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace MaxRumsey.OzStripsPlugin.GUI;

internal static class Telemetry
{
    private const string _serviceName = "ozstrips-client";

    private static string OltpEndpoint => Debugger.IsAttached ? "http://localhost:4318" : "https://oltp.maxrumsey.com"; // todo: fix

    public static readonly ActivitySource ActivitySource = new ActivitySource(_serviceName);
    public static readonly Meter Meter = new Meter(_serviceName);

    private static TracerProvider _tracerProvider;
    private static MeterProvider _meterProvider;
    private static ILoggerFactory _loggerFactory;

    public static ILoggerFactory LoggerFactory => _loggerFactory;

    public static void Initialize()
    {
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: _serviceName, serviceVersion: GetVersion())
            .AddAttributes(new[]
            {
                    new KeyValuePair<string, object>("deployment.environment", Debugger.IsAttached ? "dev" : "prod"),
                    new KeyValuePair<string, object>("host.name", Environment.MachineName),
            });

        _tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddSource(_serviceName)
            .AddHttpClientInstrumentation() // traces outbound HttpClient calls (works on netfx via System.Net.Http)
            .AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri(OltpEndpoint + "/v1/traces");
                o.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
            })
            .Build();

        _meterProvider = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddMeter(_serviceName)
            .AddRuntimeInstrumentation() // GC, thread pool, exception counts — free
            .AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri(OltpEndpoint + "/v1/metrics");
                o.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
            })
            .Build();

        _loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddOpenTelemetry(o =>
            {
                o.SetResourceBuilder(resourceBuilder);
                o.IncludeFormattedMessage = true;
                o.IncludeScopes = true;
                o.ParseStateValues = true;
                o.AddOtlpExporter(exp =>
                {
                    exp.Endpoint = new Uri(OltpEndpoint + "/v1/logs");
                    exp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                });
            });
            builder.SetMinimumLevel(Debugger.IsAttached ? LogLevel.Trace : LogLevel.Information);
        });
    }

    public static void Shutdown()
    {
        // Flush order matters less than making sure Dispose happens —
        // each provider flushes its own exporter on dispose.
        _tracerProvider?.Dispose();
        _meterProvider?.Dispose();
        _loggerFactory?.Dispose();
    }

    private static string GetVersion() =>
        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

}
