﻿namespace TypinExamples.Validation
{
    using FluentValidation;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Directives;
    using TypinExamples.Infrastructure.TypinWeb.Commands;
    using TypinExamples.Validation.Middleware;

    public class Startup : ICliStartup
    {
        public void Configure(CliApplicationBuilder app)
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;

            app.AddCommandsFromThisAssembly()
               .AddDirective<PreviewDirective>()
               .AddCommand<PipelineCommand>()
               .AddCommand<ServicesCommand>()
               .UseMiddleware<FluentValidationMiddleware>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining(typeof(Startup));
        }
    }
}
