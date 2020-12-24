namespace TypinExamples
{
    using System;
    using System.Net.Http;
    using BlazorDownloadFile;
    using Blazored.Toast;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using TypinExamples.Application.Extensions;
    using TypinExamples.Application.Handlers.Commands.Terminal;
    using TypinExamples.Application.Handlers.Notifications.LogViewer;
    using TypinExamples.Application.Services.TypinWeb;
    using TypinExamples.Configurations;
    using TypinExamples.Infrastructure.WebWorkers.Core;
    using TypinExamples.Services;
    using TypinExamples.Services.Terminal;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IWebAssemblyHostEnvironment environment)
        {
            services.AddOptions();
            services.AddBlazoredToast();

            services.AddBlazorDownloadFile();

            services.AddWebWorkers()
                    .RegisterCommandHandler<ClearCommand, ClearCommand.Handler>()
                    .RegisterCommandHandler<WriteCommand, WriteCommand.Handler>()
                    .RegisterNotificationHandler<LogNotification, LogNotification.Handler>();

            services.AddLogging(builder => builder.AddSerilog(dispose: true)
                                                  .SetMinimumLevel(environment.IsDevelopment() ? LogLevel.Trace : LogLevel.Information));

            services.AddConfiguration<ApplicationConfiguration>(configuration)
                    .AddConfiguration<HeaderConfiguration>(configuration)
                    .AddConfiguration<FooterConfiguration>(configuration);

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(environment.BaseAddress) })
                    .AddScoped<IMarkdownService, MarkdownService>()
                    .AddScoped<ITerminalRepository, TerminalRepository>()
                    .AddScoped<ILoggerDestinationRepository, LoggerDestinationRepository>()
                    .AddScoped<MonacoEditorService>();

            return services;
        }
    }
}
