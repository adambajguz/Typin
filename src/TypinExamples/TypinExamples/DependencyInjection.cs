namespace TypinExamples
{
    using System;
    using System.Net.Http;
    using Blazor.Extensions.Logging;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TypinExamples.Common.Extensions;
    using TypinExamples.Configuration;
    using TypinExamples.Services;

    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IWebAssemblyHostEnvironment environment)
        {
            services.AddOptions();

            services.AddLogging(builder => builder.AddBrowserConsole()
                                                  .SetMinimumLevel(environment.IsDevelopment() ? LogLevel.Trace : LogLevel.Information));

            services.AddConfiguration<ApplicationSettings>(configuration)
                    .AddConfiguration<HeaderSettings>(configuration)
                    .AddConfiguration<FooterSettings>(configuration)
                    .AddConfiguration<ExamplesSettings>(configuration);

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(environment.BaseAddress) })
                    .AddScoped<IMarkdownService, MarkdownService>()
                    .AddScoped<MonacoEditorService>()
                    .AddScoped<XTermService>()
                    .AddScoped<ExampleRunnerService>();

            return services;
        }
    }
}
