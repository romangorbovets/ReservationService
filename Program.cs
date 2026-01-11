using FluentValidation;
using MediatR;
using ReservationService.Application.Common.Behaviors;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Регистрация MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Регистрация FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Регистрация ValidationBehavior для автоматической валидации команд и запросов
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware для обработки ошибок (регистрируется первым, чтобы обрабатывать все исключения)
app.UseMiddleware<ReservationService.Infrastructure.Middleware.ExceptionHandlingMiddleware>();

// Middleware для логирования запросов/ответов
app.UseMiddleware<ReservationService.Infrastructure.Middleware.RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
