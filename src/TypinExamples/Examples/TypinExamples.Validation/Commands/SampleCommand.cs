namespace TypinExamples.Validation.Commands
{
    using System.Threading.Tasks;
    using FluentValidation;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command]
    public class SampleCommand : ICommand
    {
        [CommandParameter(0)]
        public string? Email { get; set; }

        [CommandParameter(1)]
        public string? Email2 { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var validator = new SampleCommandValidator();
            await validator.ValidateAndThrowAsync(this);

            await console.Output.WriteLineAsync(typeof(SampleCommand).AssemblyQualifiedName);
        }
    }
}