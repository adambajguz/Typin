namespace Typin.Tests.Data.Valid.Extensions
{
    public static class CliApplicationBuilderExtensions
    {
        public static CliApplicationBuilder AddCommandsFromValidAssembly(this CliApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.AddCommandsFromThisAssembly();
        }
    }
}
