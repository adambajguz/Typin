namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Services;

    [Command(nameof(WithDependenciesCommand))]
    public class WithDependenciesCommand : ICommand
    {
        private readonly DependencyA _dependencyA;
        private readonly DependencyB _dependencyB;
        private readonly DependencyC _dependencyC;
        private readonly IConsole _console;

        public WithDependenciesCommand(DependencyA dependencyA, DependencyB dependencyB, DependencyC dependencyC, IConsole console)
        {
            _dependencyA = dependencyA;
            _dependencyB = dependencyB;
            _dependencyC = dependencyC;
            _console = console;
        }

        public ValueTask ExecuteAsync(CancellationToken cancellationToken)
        {
            _console.Output.WriteLine($"{_dependencyA.Value}|{_dependencyB.Value}|{_dependencyC.Value}");
            _console.Output.WriteLine($"{_dependencyA.Id}|{_dependencyB.Id}|{_dependencyC.Id}");
            _console.Output.WriteLine(_dependencyC.DependencyBId);

            return default;
        }
    }
}