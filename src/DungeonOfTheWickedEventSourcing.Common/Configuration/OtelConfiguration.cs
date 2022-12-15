﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DungeonOfTheWickedEventSourcing.Common.Akka.Configuration
{
    public static class OtelConfiguration
    {
        public const string ApplicationName = "dotwes";
        private static readonly ResourceBuilder _resourceBuilder = ResourceBuilder.CreateDefault().AddService(ApplicationName);
        private static readonly Uri _otlpExporterEndpointUri = new Uri("http://collector:4317");

        public static IServiceCollection ConfigureOpenTelemetryDiagnostics(this IServiceCollection services)
        {
            services.AddOpenTelemetryTracing(configure => configure
                .SetResourceBuilder(_resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddSource(ApplicationName)                
                .AddConsoleExporter()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = _otlpExporterEndpointUri;
                }));

            services.AddOpenTelemetryMetrics(configure => configure
                .SetResourceBuilder(_resourceBuilder)
                .AddMeter("Meter")                
                .AddConsoleExporter()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = _otlpExporterEndpointUri;
                }));

            return services;
        }

        public static ILoggingBuilder ConfigureOpenTelemetryDiagnostics(this ILoggingBuilder logging)
        {
            logging.AddOpenTelemetry(configure =>
            {
                configure.SetResourceBuilder(_resourceBuilder);
                configure.IncludeFormattedMessage = true;
                configure.IncludeScopes = true;
                configure.ParseStateValues = true;
                configure.AddConsoleExporter();
                configure.AddOtlpExporter(options =>
                {
                    options.Endpoint = _otlpExporterEndpointUri;
                });
            });

            return logging;
        }
    }
}