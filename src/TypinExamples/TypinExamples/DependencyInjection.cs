namespace TypinExamples
{
    using System;
    using System.Net.Http;
    using Blazor.Extensions.Logging;
    using Blazor.Extensions.Storage;
    using BlazorWorker.Core;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TypinExamples.Common.Extensions;
    using TypinExamples.Configuration;
    using TypinExamples.Core.Configuration;
    using TypinExamples.Core.Services;
    using TypinExamples.Services;
    using TypinExamples.Services.Terminal;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IWebAssemblyHostEnvironment environment)
        {
            services.AddOptions();
            services.AddStorage();
            services.AddWorkerFactory();

            services.AddLogging(builder => builder.AddBrowserConsole()
                                                  .SetMinimumLevel(environment.IsDevelopment() ? LogLevel.Trace : LogLevel.Information));

            services.AddConfiguration<ApplicationSettings>(configuration)
                    .AddConfiguration<HeaderSettings>(configuration)
                    .AddConfiguration<FooterSettings>(configuration);

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(environment.BaseAddress) })
                    .AddScoped<IMarkdownService, MarkdownService>()
                    .AddSingleton<ITerminalRepository, TerminalRepository>()
                    .AddScoped<MonacoEditorService>();

            return services;
        }
    }
}
