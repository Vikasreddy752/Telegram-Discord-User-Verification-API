using Microsoft.EntityFrameworkCore;
using TelegramAPI.Data;
using YourNamespace.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // your Angular app URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
// Bind Telegram settings
builder.Services.Configure<TelegramSettings>(
    builder.Configuration.GetSection("TelegramSettings"));
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


// Register HttpClient
builder.Services.AddHttpClient();

// Add controllers
builder.Services.AddControllers();

builder.Services.AddDbContext<Appdb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowAngularApp");
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
