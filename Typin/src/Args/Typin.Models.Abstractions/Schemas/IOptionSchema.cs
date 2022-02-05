namespace Typin.Models.Schemas
{
    using System;

    /// <summary>
    /// Option schema.
    /// </summary>
    public interface IOptionSchema : IArgumentSchema
    {
        /// <summary>
        /// Option short name.
        /// </summary>
        char? ShortName { get; }

        /// <summary>
        /// Whether option is required.
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        /// Gets a call name.
        /// </summary>
        /// <returns></returns>
        string GetCallName();

        /// <summary>
        /// Whether options's name matches the passed name.
        /// </summary>
        bool MatchesName(string name);

        /// <summary>
        /// Whether options's short name matches the passed short name.
        /// </summary>
        bool MatchesShortName(char shortName);

        /// <summary>
        /// Whether options's name or short name matches the passed alias.
        /// </summary>
        bool MatchesNameOrShortName(string alias);

        /// <summary>
        /// Checks whether text is a valid option name.
        /// </summary>
        public static bool IsName(string name)
        {
            return name.StartsWith("--", StringComparison.Ordinal) &&
                name.Length >= 3 &&
                char.IsLetter(name[2]);
        }

        /// <summary>
        /// Checks whether text is a valid option short name.
        /// </summary>
        public static bool IsShortName(string shortName)
        {
            return shortName.StartsWith('-') &&
                shortName.Length == 2 &&
                char.IsLetter(shortName[1]);
        }

        /// <summary>
        /// Checks whether text is a valid option short name.
        /// </summary>
        public static bool IsShortNameGroup(string shortNameGroup)
        {
            return shortNameGroup.StartsWith('-') &&
                shortNameGroup.Length >= 2 &&
                char.IsLetter(shortNameGroup[1]);
        }
    }
}