namespace Typin.OptionFallback
{
    using System.Collections.Generic;

    /// <summary>
    /// Option fallback value provider.
    /// </summary>
    public interface IOptionFallbackProvider : IReadOnlyDictionary<string, string>
    {
        //TODO: maybe IReadOnlyDictionary<string, object>
        //or maybe only IReadOnlyCollection
        //or maybe TryGetValue(TKey key, Type propertyType, out TValue value)
        //or TryGetValue(TKey key, object propertyDefaultValue, out TValue value)
        //or maybe fallback values store is a bad concept
        //or maybe not a bad concept since this can be shown in help?
    }
}
