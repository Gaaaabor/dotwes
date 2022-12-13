using DungeonOfTheWickedEventSourcing.Api;
using Microsoft.AspNetCore.ResponseCompression;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public class Program
{
    public const string ApplicationName = "dotwes";    

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHostedService<AkkaHost>();
        builder.Services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
        });

        builder.Services.AddOpenTelemetryTracing(configure => configure
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ApplicationName))
            .AddAspNetCoreInstrumentation()            
            .AddConsoleExporter()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://dotwes-otel-collector:4317");
            }));

        builder.Services.AddOpenTelemetryMetrics(configure => configure
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ApplicationName))
            .AddMeter("Meter")
            .AddConsoleExporter()                        
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://dotwes-otel-collector:4317");
            }));

        builder.Logging.AddOpenTelemetry(configure =>
        {
            configure.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ApplicationName));
            configure.IncludeFormattedMessage = true;
            configure.IncludeScopes = true;
            configure.ParseStateValues = true;
            configure.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://dotwes-otel-collector:4317");
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseWebSockets();

        app.UseResponseCompression();

        app.Run();
    }
}