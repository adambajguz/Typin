namespace Typin.OptionFallback
{
    using System.Collections.Generic;
    using Typin.Attributes;

    /// <summary>
    /// Fallback value store. Values stored in class implementing this interaface will be used as fallback if no option value is specified (<see cref="CommandOptionAttribute"/>).
    /// </summary>
    public interface IFallbackValuesStore : IReadOnlyDictionary<string, string>
    {
        //TODO: maybe IReadOnlyDictionary<string, object>
        //or maybe only IReadOnlyCollection
        //or maybe TryGetValue(TKey key, Type propertyType, out TValue value)
        //or TryGetValue(TKey key, object propertyDefaultValue, out TValue value)
        //or maybe fallback values store is a bad concept
        //or maybe not a bad concept since this can be shown in help?
    }
}
