using DungeonOfTheWickedEventSourcing.Api;
using DungeonOfTheWickedEventSourcing.Api.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IActorSystem, AkkaHost>();
builder.Services.AddHostedService(x => (AkkaHost)x.GetRequiredService<IActorSystem>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
