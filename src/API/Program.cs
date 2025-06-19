using Models;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection") 
              ?? throw new InvalidOperationException("DefaultConnection connection string configuration is missing.");


builder.Services.AddTransient<IPotatoTeacherRepository, PotatoTeacherRepository>();
builder.Services.AddTransient<IQuizRepository, QuizRepository>(_ => new QuizRepository(connStr!));
builder.Services.AddTransient<IQuizTeacherRepository, QuizTeacherRepository>(_ => new QuizTeacherRepository(connStr!));
builder.Services.AddTransient<IPotatoTeacherService, PotatoTeacherService>();

builder.Services.AddControllers();
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