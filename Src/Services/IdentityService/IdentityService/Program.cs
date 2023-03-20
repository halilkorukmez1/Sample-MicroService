using IdentityService.Config;
using IdentityService.Config.DbSettings;
using IdentityService.Data;
using IdentityService.Data.Context;
using IdentityService.Helpers;
using IdentityService.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<IdentityDatabaseSettings>(builder.Configuration.GetSection(nameof(IdentityDatabaseSettings)));
builder.Services.AddSingleton<IIdentityDatabaseSettings>(sp => sp.GetRequiredService<IOptions<IdentityDatabaseSettings>>().Value);
builder.Services.AddSingleton<IIdentityContext, IdentityContext>();
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI();}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<JwtMiddleware>();
app.Run();
