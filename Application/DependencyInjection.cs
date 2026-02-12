using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Common.MediatR;
using ReservationService.Application.Common.Services;
using ReservationService.Application.Common.Settings;

namespace ReservationService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICommandSender, CommandSender>();

        return services;
    }

    public static IServiceCollection RegisterCommandAdapters(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        var commandHandlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface &&
                       t.GetInterfaces()
                           .Any(i => i.IsGenericType &&
                                    (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                                     i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))))
            .ToList();

        foreach (var handlerType in commandHandlerTypes)
        {
            var interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType &&
                           (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
                .ToList();

            foreach (var interfaceType in interfaces)
            {
                
                services.AddScoped(interfaceType, handlerType);

                
                var genericArgs = interfaceType.GetGenericArguments();
                if (genericArgs.Length == 2)
                {
                    var commandType = genericArgs[0];
                    var responseType = genericArgs[1];
                    var adapterType = typeof(CommandHandlerMediatRAdapter<,>).MakeGenericType(commandType, responseType);
                    var requestType = typeof(CommandMediatRAdapter<,>).MakeGenericType(commandType, responseType);
                    var handlerInterface = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
                    services.AddScoped(handlerInterface, sp =>
                    {
                        var commandHandler = sp.GetRequiredService(interfaceType);
                        return Activator.CreateInstance(adapterType, commandHandler)!;
                    });
                }
                else if (genericArgs.Length == 1)
                {
                    var commandType = genericArgs[0];
                    var adapterType = typeof(CommandHandlerMediatRAdapter<>).MakeGenericType(commandType);
                    var requestType = typeof(CommandMediatRAdapter<>).MakeGenericType(commandType);
                    var handlerInterface = typeof(IRequestHandler<>).MakeGenericType(requestType);
                    services.AddScoped(handlerInterface, sp =>
                    {
                        var commandHandler = sp.GetRequiredService(interfaceType);
                        return Activator.CreateInstance(adapterType, commandHandler)!;
                    });
                }
            }
        }

        return services;
    }
}