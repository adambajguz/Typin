namespace Typin.Internal.Pipeline
{
    internal static class CorePipelineCliBuilderExtensions
    {
        public static CliApplicationBuilder AddAfterInputParseMiddlewares(this CliApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResolveCommandSchema>()
                          .UseMiddleware<ResolveCommandInstance>()
                          .UseMiddleware<InitializeDirectives>();
        }

        public static CliApplicationBuilder AddAfterUserMiddlewares(this CliApplicationBuilder builder)
        {
            return builder.UseMiddleware<HandleSpecialOptions>()
                          .UseMiddleware<ExecuteDirectivesSubpipeline>()
                          .UseMiddleware<BindInputAndExecuteCommand>();
        }
    }
}
