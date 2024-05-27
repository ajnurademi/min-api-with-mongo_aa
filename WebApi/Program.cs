var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/check", () => {
/* Code zur Prüfung der DB ...*/
return "Minimal API Version 1.1";
});

app.Run();
