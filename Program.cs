using hackathon_file_import.Core.Interfaces;
using hackathon_file_import.Core.Services;
using hackathon_file_import.Infrastructure.DataAccess;
using hackathon_file_import.Infrastructure.Repositories;
using hackaton_file_import.common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var appSettingsSection = builder.Configuration.GetSection(AppSettings.SectionName);
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();


var key = Encoding.ASCII.GetBytes(appSettings.Secret);
builder.Services
    .AddAuthentication(configuration =>
    {
        configuration.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        configuration.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(configuration =>
    {
        configuration.Events = new JwtBearerEvents
        {
            OnTokenValidated = context => {
                var userId = context.Principal?.Identity?.Name;
                if (userId == null)
                {
                    context.Fail("Unauthorized");
                    return Task.CompletedTask;
                }

                return Task.CompletedTask;
            }
        };
        configuration.RequireHttpsMetadata = false;
        configuration.SaveToken = true;
        configuration.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    })
    .AddCookie();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoDbSection = builder.Configuration.GetSection(MongoDbSettings.SectionName);
builder.Services.Configure<MongoDbSettings>(mongoDbSection);
var mongoDbOptions = mongoDbSection.Get<MongoDbSettings>();

builder.Services.AddSingleton<MongoDbContext>(provider =>
    new MongoDbContext(
        builder.Configuration.GetConnectionString("MongoDB"),
        builder.Configuration.GetValue<string>("DatabaseName")));

builder.Services.AddTransient<IFileImportService, FileImportService>();
builder.Services.AddTransient<IRepository<byte[]>, BlobRepository>();
var app = builder.Build();

var tokenValidationSettings = builder.Configuration.GetSection(TokenValidationSettings.SectionName);
builder.Services.Configure<TokenValidationSettings>(tokenValidationSettings);
var servicesUrlsOptions = tokenValidationSettings.Get<TokenValidationSettings>();

builder.Services.AddHttpClient("oauth", httpClient =>
{
    httpClient.BaseAddress = new Uri(servicesUrlsOptions.OAuthServiceUrl);
    httpClient.DefaultRequestHeaders.ConnectionClose = true;
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();

