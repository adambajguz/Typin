namespace Typin.Tests.Commands.Valid
{
    using System;

    public class DependencyA
    {
        public char Value { get; set; } = 'A';
        public Guid Id { get; } = Guid.NewGuid();

        public DependencyA()
        {

        }
    }
}