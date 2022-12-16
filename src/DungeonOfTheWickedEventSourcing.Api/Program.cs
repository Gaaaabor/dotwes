using DungeonOfTheWickedEventSourcing.Api;
using DungeonOfTheWickedEventSourcing.Api.Application;
using DungeonOfTheWickedEventSourcing.Api.Hubs;
using DungeonOfTheWickedEventSourcing.Common.Configuration;
using Microsoft.AspNetCore.ResponseCompression;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<AkkaHost>();
        builder.Services.AddSingleton<ISignalRProcessor>(x => x.GetRequiredService<AkkaHost>());
        builder.Services.AddSingleton<IGraphDataProvider>(x => x.GetRequiredService<AkkaHost>());
        builder.Services.AddHostedService(x => x.GetRequiredService<AkkaHost>());
        builder.Services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
        });

        builder.Services.ConfigureOpenTelemetryDiagnostics();
        builder.Logging.ConfigureOpenTelemetryDiagnostics();

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
        app.MapHub<MainHub>(MainHub.Path);

        app.MapGet("/api/graph/fields", async x =>
        {
            var akkaHost = app.Services.GetRequiredService<IGraphDataProvider>();
            var graphFields = akkaHost.GetGraphFields();
            await x.Response.WriteAsync(graphFields);
        });

        app.MapGet("/api/graph/data", async x =>
        {
            var akkaHost = app.Services.GetRequiredService<IGraphDataProvider>();
            var graphData = await akkaHost.GetGraphData();
            await x.Response.WriteAsync(graphData);
        });

        app.MapGet("/api/health", async x =>
        {
            await x.Response.WriteAsJsonAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        });

        app.UseResponseCompression();

        app.Run();
    }
}