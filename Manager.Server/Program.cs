using Manager.Server;

var builder = WebApplication.CreateBuilder(args);
var AllowSpecificOrigins = "_AllowSpecificOrigins";

Cache cache = Cache.Instance;

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
            "https://localhost:4200"
        ).AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

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
