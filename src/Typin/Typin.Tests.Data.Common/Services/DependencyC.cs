namespace Typin.Tests.Data.Services
{
    using System;

    public class DependencyC
    {
        public char Value { get; } = 'C';
        public Guid Id { get; } = Guid.NewGuid();
        public Guid DependencyBId => _dependencyB.Id;


        private readonly DependencyB _dependencyB;

        public DependencyC(DependencyB dependencyB)
        {
            _dependencyB = dependencyB;
            Value = dependencyB.Value;
        }
    }
}