var builder = WebApplication.CreateBuilder(args);

// HTTP endpoints will be organized as controllers as the API grows.
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
