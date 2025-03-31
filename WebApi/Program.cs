using ApplicationCore.Commons.Repository;
using ApplicationCore.Interfaces.AdminService;
using ApplicationCore.Interfaces.UserService;
using ApplicationCore.Models;
using ApplicationCore.Models.QuizAggregate;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Memory.Generators;
using Infrastructure.Memory.Repositories;
using Web;
using WebApi.Dto;
using WebApi.Mapper;
using WebApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddTransient<IGenericGenerator<int>, IntGenerator>();
builder.Services.AddSingleton<IGenericRepository<Quiz, int>, MemoryGenericRepository<Quiz, int>>();
builder.Services.AddSingleton<IGenericRepository<QuizItem, int>, MemoryGenericRepository<QuizItem, int>>();
builder.Services.AddSingleton<IGenericRepository<QuizItemUserAnswer, string>, MemoryGenericRepository<QuizItemUserAnswer, string>>();
builder.Services.AddSingleton<IQuizUserService, QuizUserService>();
builder.Services.AddSingleton<IQuizAdminService, QuizAdminService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<QuizItemDto>, QuizItemValidator>();
builder.Services.AddControllers().AddNewtonsoftJson();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Seed();
app.Run();
