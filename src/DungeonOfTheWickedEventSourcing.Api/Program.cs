using DungeonOfTheWickedEventSourcing.Api;
using DungeonOfTheWickedEventSourcing.Common.Actors.SignalR;
using DungeonOfTheWickedEventSourcing.Common.Configuration;
using DungeonOfTheWickedEventSourcing.Common.Hubs;
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

        app.MapGet("/api/health", async x =>
        {
            await x.Response.WriteAsJsonAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        });

        app.UseResponseCompression();

        app.Run();
    }
}