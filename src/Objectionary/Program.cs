var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.MapGet("/api/health", () => Results.Ok());

app.Run();
