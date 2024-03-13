using Infrastructure;
using MediatR;
using System.Reflection;
using Application;
using Hangfire;
using Hangfire.PostgreSql;
using API.Jobs;

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

//builder.Services.AddHangfire(config => config.UsePostgreSqlStorage("Host=localhost;Port=5432;Database=postgres;User Id=sa;Password=Test123;"));
builder.Services.AddHangfire(config =>
{
    var hangfireConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    config.UsePostgreSqlStorage(hangfireConnectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("Open");

app.UseHangfireServer();
app.UseHangfireDashboard();
app.MapHangfireDashboard("/hangfire");

//RecurringJob.AddOrUpdate<CurrencyRatesJob>("get-currency-rates", x => x.GetFromApi(), "*/5 * * * *"); - co 5 minut
RecurringJob.AddOrUpdate<CurrencyRatesJob>("get-currency-rates", x => x.GetFromApi(), Cron.Hourly); // co godzine

app.Run();
