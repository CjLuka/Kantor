using Infrastructure;
using MediatR;
using System.Reflection;
using Application;
using Hangfire;
using Hangfire.PostgreSql;
using API.Jobs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddHttpClient();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open",
        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddHangfire(config =>
{
    var hangfireConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    config.UsePostgreSqlStorage(hangfireConnectionString);
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    //.WriteTo.Console()
    .WriteTo.File("logs/myLogs-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

//DO LOGOWANIA KA¯DEJ AKCJI
//builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//DO LOGOWANIA KA¯DEJ AKCJI
//app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("Open");

app.UseHangfireServer();
app.UseHangfireDashboard();
app.MapHangfireDashboard("/hangfire");

//RecurringJob.AddOrUpdate<CurrencyRatesJob>("get-currency-rates", x => x.GetFromApi(), "*/3 * * * *");// - co 5 minut
RecurringJob.AddOrUpdate<CurrencyRatesJob>("get-currency-rates", x => x.GetFromApi(), Cron.Hourly); // co godzine

app.Run();
public partial class Program { }
