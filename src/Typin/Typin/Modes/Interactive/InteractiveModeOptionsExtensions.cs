namespace Typin.Modes
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Console;

    /// <summary>
    /// <see cref="InteractiveModeOptions"/> extensions.
    /// </summary>
    public static class InteractiveModeOptionsExtensions
    {
        //TODO: add tests

        /// <summary>
        /// Sets interactive mode prompt to action that directly interacts with console and has a DI resolvable service dependency.
        /// It is recommended to use <see cref="InteractiveModeOptions.PromptForeground"/> and <see cref="InteractiveModeOptions.ScopeForeground"/>.
        /// </summary>
        public static InteractiveModeOptions SetPrompt<TDep>(
            this InteractiveModeOptions options,
            Action<TDep, ApplicationMetadata, IConsole> prompt)
            where TDep : notnull
        {
            options.SetPrompt((provider, metadata, console) =>
            {
                TDep dep = provider.GetRequiredService<TDep>();

                prompt(dep, metadata, console);
            });

            return options;
        }

        /// <summary>
        /// Sets interactive mode prompt to action that directly interacts with console and has a DI resolvable service dependency.
        /// It is recommended to use <see cref="InteractiveModeOptions.PromptForeground"/> and <see cref="InteractiveModeOptions.ScopeForeground"/>.
        /// </summary>
        public static InteractiveModeOptions SetPrompt<TDep>(
            this InteractiveModeOptions options,
            Action<TDep, InteractiveModeOptions, ApplicationMetadata, IConsole> prompt)
            where TDep : notnull
        {
            options.SetPrompt((provider, metadata, console) =>
            {
                TDep dep = provider.GetRequiredService<TDep>();

                prompt(dep, options, metadata, console);
            });

            return options;
        }

        /// <summary>
        /// Sets interactive mode prompt to action that directly interacts with console and has a DI resolvable service dependency.
        /// It is recommended to use <see cref="InteractiveModeOptions.PromptForeground"/> and <see cref="InteractiveModeOptions.ScopeForeground"/>.
        /// </summary>
        public static InteractiveModeOptions SetPrompt<TDep1, TDep2>(
            this InteractiveModeOptions options,
            Action<TDep1, TDep2, ApplicationMetadata, IConsole> prompt)
            where TDep1 : notnull
            where TDep2 : notnull
        {
            options.SetPrompt((provider, metadata, console) =>
            {
                TDep1 dep1 = provider.GetRequiredService<TDep1>();
                TDep2 dep2 = provider.GetRequiredService<TDep2>();

                prompt(dep1, dep2, metadata, console);
            });

            return options;
        }

        /// <summary>
        /// Sets interactive mode prompt to action that directly interacts with console and has a DI resolvable service dependency.
        /// It is recommended to use <see cref="InteractiveModeOptions.PromptForeground"/> and <see cref="InteractiveModeOptions.ScopeForeground"/>.
        /// </summary>
        public static InteractiveModeOptions SetPrompt<TDep1, TDep2>(
            this InteractiveModeOptions options,
            Action<TDep1, TDep2, InteractiveModeOptions, ApplicationMetadata, IConsole> prompt)
            where TDep1 : notnull
            where TDep2 : notnull
        {
            options.SetPrompt((provider, metadata, console) =>
            {
                TDep1 dep1 = provider.GetRequiredService<TDep1>();
                TDep2 dep2 = provider.GetRequiredService<TDep2>();

                prompt(dep1, dep2, options, metadata, console);
            });

            return options;
        }

        /// <summary>
        /// Sets interactive mode prompt to action that directly interacts with console and has a DI resolvable service dependency.
        /// It is recommended to use <see cref="InteractiveModeOptions.PromptForeground"/> and <see cref="InteractiveModeOptions.ScopeForeground"/>.
        /// </summary>
        public static InteractiveModeOptions SetPrompt<TDep1, TDep2, TDep3>(
            this InteractiveModeOptions options,
            Action<TDep1, TDep2, TDep3, ApplicationMetadata, IConsole> prompt)
            where TDep1 : notnull
            where TDep2 : notnull
            where TDep3 : notnull
        {
            options.SetPrompt((provider, metadata, console) =>
            {
                TDep1 dep1 = provider.GetRequiredService<TDep1>();
                TDep2 dep2 = provider.GetRequiredService<TDep2>();
                TDep3 dep3 = provider.GetRequiredService<TDep3>();

                prompt(dep1, dep2, dep3, metadata, console);
            });

            return options;
        }

        /// <summary>
        /// Sets interactive mode prompt to action that directly interacts with console and has a DI resolvable service dependency.
        /// It is recommended to use <see cref="InteractiveModeOptions.PromptForeground"/> and <see cref="InteractiveModeOptions.ScopeForeground"/>.
        /// </summary>
        public static InteractiveModeOptions SetPrompt<TDep1, TDep2, TDep3>(
            this InteractiveModeOptions options,
            Action<TDep1, TDep2, TDep3, InteractiveModeOptions, ApplicationMetadata, IConsole> prompt)
            where TDep1 : notnull
            where TDep2 : notnull
            where TDep3 : notnull
        {
            options.SetPrompt((provider, metadata, console) =>
            {
                TDep1 dep1 = provider.GetRequiredService<TDep1>();
                TDep2 dep2 = provider.GetRequiredService<TDep2>();
                TDep3 dep3 = provider.GetRequiredService<TDep3>();

                prompt(dep1, dep2, dep3, options, metadata, console);
            });

            return options;
        }

    }
}
