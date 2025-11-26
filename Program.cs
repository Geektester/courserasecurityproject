using SafeVault;
using SafeVault.Models;
using SafeVault.Services;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
builder.Services.AddControllers();
AuthConfig.ConfigureAuth(builder.Services, builder.Configuration);

var app = builder.Build();

// Middleware pipeline
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
