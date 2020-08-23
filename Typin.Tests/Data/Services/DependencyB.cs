namespace Typin.Tests.Data.Services
{
    using System;

    public class DependencyB
    {
        public char Value { get; set; } = 'B';
        public Guid Id { get; } = Guid.NewGuid();

        public DependencyB()
        {

        }
    }
}