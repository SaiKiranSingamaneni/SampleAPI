using ProductivityOptimizerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddScoped<IProductivityService, ProductivityService>();
builder.Services.AddScoped<IFocusScoreService, FocusScoreService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

var port = app.Configuration["PORT"] ?? "8080";
app.Run($"http://localhost:{port}");
