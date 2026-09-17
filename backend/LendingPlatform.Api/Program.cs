using System.Text.Json.Serialization;
using LendingPlatform.Api.Services;
using LendingPlatform.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSingleton<LoanDecisionService>();
builder.Services.AddSingleton<LoanApplicationStore>();

var app = builder.Build();

app.MapControllers();

app.Run();
