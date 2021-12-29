namespace Typin.Models.Attributes
{
    using System;

    /// <summary>
    /// Represents an argument attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class ArgumentAttribute : Attribute
    {

    }
}