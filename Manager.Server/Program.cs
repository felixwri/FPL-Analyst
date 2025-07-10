using Manager.Server.Interfaces;
using Manager.Server.Services;
using Manager.Server.Source;
using Manager.Server.Startup;


var builder = WebApplication.CreateBuilder(args);
var AllowSpecificOrigins = "_AllowSpecificOrigins";

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IHttpFetchService, HttpFetchService>();
builder.Services.AddScoped<ILiveDataService, LiveDataService>();
builder.Services.AddSingleton<Cache>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add cors policy to origins
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins, policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "https://127.0.0.1:4200",
            "https://localhost:4200",
            "http://192.168.1.15:4200"
        ).AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

await CacheInit.InitialiseAsync(app.Services);

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(AllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
