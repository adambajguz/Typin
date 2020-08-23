namespace Typin.Tests.Data.Commands.Valid
{
    using System.Threading.Tasks;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Services;

    [Command("cmd")]
    public class WithDependenciesCommand : ICommand
    {
        private readonly DependencyA _dependencyA;
        private readonly DependencyB _dependencyB;
        private readonly DependencyC _dependencyC;

        public WithDependenciesCommand(DependencyA dependencyA, DependencyB dependencyB, DependencyC dependencyC)
        {
            _dependencyA = dependencyA;
            _dependencyB = dependencyB;
            _dependencyC = dependencyC;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine($"{_dependencyA.Value}|{_dependencyB.Value}|{_dependencyC.Value}");
            console.Output.WriteLine($"{_dependencyA.Id}|{_dependencyB.Id}|{_dependencyC.Id}");
            console.Output.WriteLine(_dependencyC.DependencyBId);

            return default;
        }
    }
}