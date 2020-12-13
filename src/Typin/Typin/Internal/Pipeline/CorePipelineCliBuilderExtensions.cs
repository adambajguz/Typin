namespace Typin.Internal.Pipeline
{
    internal static class CorePipelineCliBuilderExtensions
    {
        public static CliApplicationBuilder AddBeforeUserMiddlewares(this CliApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResolveCommandSchemaAndInstance>()
                          .UseMiddleware<InitializeDirectives>()
                          .UseMiddleware<ExecuteDirectivesSubpipeline>()
                          .UseMiddleware<HandleSpecialOptions>()
                          .UseMiddleware<BindInput>();
        }

        public static CliApplicationBuilder AddAfterUserMiddlewares(this CliApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExecuteCommand>();
        }
    }
}
