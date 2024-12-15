using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NoPersaService.Database;
using NoPersaService.Services;
using SharedLibrary.FluentValidations;
using SharedLibrary.MappingProfiles;
using SharedLibrary.Util;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};";

builder.Services.AddDbContext<NoPersaDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

foreach (var validator in SharedLibrary.Util.ProgramBuilder.GetFluentValidations())
{
    builder.Services.AddScoped(validator.Item1, validator.Item2);
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(typeof(ManagementProfile), typeof(DeliveryProfile), typeof(GastronomyProfile), typeof(DefaultProfile));
builder.Services.AddHostedService<DailyDeliveryService>();
builder.Services.AddHostedService<ArticleService>();
builder.Services.AddHostedService<CustomersBoxStatusService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NoPersaDbContext>();
    db.Database.Migrate();
    db.SeedData();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseAuthorization();
app.MapControllers();
app.Run();