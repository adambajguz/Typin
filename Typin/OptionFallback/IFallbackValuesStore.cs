namespace Typin.OptionFallback
{
    using System.Collections.Generic;
    using Typin.Attributes;

    /// <summary>
    /// Fallback value store. Values stored in class implementing this interaface will be used as fallback if no option value is specified (<see cref="CommandOptionAttribute"/>).
    /// </summary>
    public interface IFallbackValuesStore : IReadOnlyDictionary<string, string>
    {

    }
}
