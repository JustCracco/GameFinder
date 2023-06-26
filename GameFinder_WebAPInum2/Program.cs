using Microsoft.EntityFrameworkCore;
using GameFinder_WebAPI.Database;
using GameFinder_WebAPI.Services.IServices;
using GameFinder_WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<GameDb>(opt =>
    opt.UseInMemoryDatabase("GameList"));
builder.Services.AddTransient<IGameService, GameService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
