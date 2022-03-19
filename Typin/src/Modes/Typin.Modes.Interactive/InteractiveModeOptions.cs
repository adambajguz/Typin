namespace Typin.Modes.Interactive
{
    using System;
    using Typin;
    using Typin.Console;

    /// <summary>
    /// Interactive mode options.
    /// </summary>
    public class InteractiveModeOptions
    {
        /// <summary>
        /// Prompt foreground color.
        /// Default color is <see cref="ConsoleColor.Blue"/>.
        /// </summary>
        public ConsoleColor PromptForeground { get; set; } = ConsoleColor.Blue;

        /// <summary>
        /// Command input foreground color.
        /// Default color is <see cref="ConsoleColor.Yellow"/>.
        /// </summary>
        public ConsoleColor CommandForeground { get; set; } = ConsoleColor.Yellow;

        /// <summary>
        /// Prompt writer delegate. Use SetPrompt to change prompt specification (by default initialized with a call to <see cref="SetDefaultPrompt"/>).
        /// </summary>
        public Action<IServiceProvider, ApplicationMetadata, IConsole> Prompt { get; private set; } = default!;

        /// <summary>
        /// Initializes an instance of <see cref="InteractiveModeOptions"/>.
        /// </summary>
        public InteractiveModeOptions()
        {
            SetDefaultPrompt();
        }

        #region Prompt setters
        /// <summary>
        /// Sets interactive mode prompt to default ("{metadata.ExecutableName}> ").
        /// </summary>
        public InteractiveModeOptions SetDefaultPrompt()
        {
            Prompt = (provider, metadata, console) =>
            {
                ConsoleColor promptForeground = PromptForeground;
                console.Output.WithForegroundColor(promptForeground, (output) => output.Write(metadata.ExecutableName));

                //string scope = Scope;
                //bool hasScope = !string.IsNullOrWhiteSpace(scope);

                //if (hasScope)
                //{
                //    console.Output.WithForegroundColor(ConsoleColor.Cyan, (output) =>
                //    {
                //        output.Write(' ');
                //        output.Write(scope);
                //    });
                //}

                console.Output.WithForegroundColor(promptForeground, (output) => output.Write("> "));
            };

            return this;
        }

        /// <summary>
        /// Sets interactive mode prompt to simple string with foreground set to <see cref="PromptForeground"/>.
        /// </summary>
        public InteractiveModeOptions SetPrompt(string prompt)
        {
            Prompt = (provider, metadata, console) =>
            {
                console.Output.WithForegroundColor(PromptForeground, (output) => output.Write(prompt));
            };

            return this;
        }

        /// <summary>
        /// Sets interactive mode prompt to a string template that may use <see cref="ApplicationMetadata"/> with foreground set to <see cref="PromptForeground"/>.
        /// </summary>
        public InteractiveModeOptions SetPrompt(Func<ApplicationMetadata, string> prompt)
        {
            Prompt = (provider, metadata, console) =>
            {
                string tmp = prompt(metadata);

                console.Output.WithForegroundColor(PromptForeground, (output) => output.Write(tmp));
            };

            return this;
        }

        /// <summary>
        /// Sets interactive mode prompt to action that directly interacts with console.
        /// It is recommended to use <see cref="PromptForeground"/>.
        /// </summary>
        public InteractiveModeOptions SetPrompt(Action<ApplicationMetadata, IConsole> prompt)
        {
            Prompt = (provider, metadata, console) => prompt(metadata, console);

            return this;
        }

        /// <summary>
        /// Sets interactive mode prompt to action that directly interacts with console.
        /// It is recommended to use <see cref="PromptForeground"/>.
        /// </summary>
        public InteractiveModeOptions SetPrompt(Action<InteractiveModeOptions, ApplicationMetadata, IConsole> prompt)
        {
            Prompt = (provider, metadata, console) => prompt(this, metadata, console);

            return this;
        }

        /// <summary>
        /// Sets interactive mode prompt to action that directly interacts with console.
        /// It is recommended to use <see cref="PromptForeground"/>.
        /// </summary>
        public InteractiveModeOptions SetPrompt(Action<IServiceProvider, ApplicationMetadata, IConsole> prompt)
        {
            Prompt = prompt;

            return this;
        }

        /// <summary>
        /// Sets interactive mode prompt to action that directly interacts with console.
        /// It is recommended to use <see cref="PromptForeground"/>.
        /// </summary>
        public InteractiveModeOptions SetPrompt(Action<IServiceProvider, InteractiveModeOptions, ApplicationMetadata, IConsole> prompt)
        {
            Prompt = (provider, metadata, console) => prompt(provider, this, metadata, console);

            return this;
        }
        #endregion
    }
}
