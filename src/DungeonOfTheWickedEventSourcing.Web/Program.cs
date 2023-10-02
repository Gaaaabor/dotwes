using DungeonOfTheWickedEventSourcing.GameEngine.Assets;
using DungeonOfTheWickedEventSourcing.GameEngine.Assets.Loaders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//builder.RootComponents.Add<App>("app");

builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7050") });
builder.Services.AddSingleton<IAssetsResolver, AssetsResolver>();
builder.Services.AddSingleton<IAssetLoader<Sprite>, SpriteAssetLoader>();
builder.Services.AddSingleton<IAssetLoader<SpriteSheet>, SpriteSheetAssetLoader>();
builder.Services.AddSingleton<IAssetLoaderFactory>(ctx =>
{
    var factory = new AssetLoaderFactory();

    factory.Register(ctx.GetRequiredService<IAssetLoader<Sprite>>());
    factory.Register(ctx.GetRequiredService<IAssetLoader<SpriteSheet>>());

    return factory;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.UseWebSockets();

app.Run();
