namespace Typin.Tests.Commands.Valid
{
    using System;

    public class DependencyC
    {
        public char Value { get; set; } = 'C';
        public Guid Id { get; } = Guid.NewGuid();

        private readonly DependencyB _dependencyB;

        public DependencyC(DependencyB dependencyB)
        {
            _dependencyB = dependencyB;
            Value = dependencyB.Value;
        }
    }
}