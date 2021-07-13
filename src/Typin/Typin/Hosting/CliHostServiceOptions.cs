namespace Typin.Hosting
{
    using System;

    internal class CliHostServiceOptions
    {
        public Action<IApplicationBuilder>? ConfigureApplication { get; set; }
    }
}
