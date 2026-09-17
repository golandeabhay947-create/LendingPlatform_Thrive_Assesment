using System.Text.Json.Serialization;
using LendingPlatform.Api.Services;
using LendingPlatform.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddSingleton<LoanDecisionService>();
builder.Services.AddSingleton<LoanApplicationStore>();

var app = builder.Build();

app.UseCors("Frontend");
app.MapControllers();

app.Run();
