namespace Typin.Models.Builders
{
    using System;
    using Typin.Models.Converters;
    using Typin.Models.Schemas;
    using Typin.Schemas.Builders;

    /// <summary>
    /// Model option builder.
    /// </summary>
    public interface IOptionBuilder : IBuilder<IOptionSchema>, IManageExtensions<IOptionBuilder>
    {
        /// <summary>
        /// Sets option name to default (kebab-cased property name).
        /// </summary>
        /// <returns></returns>
        public IOptionBuilder DefaultName()
        {
            return Name(null);
        }

        /// <summary>
        /// Sets option short name to default (no short name).
        /// </summary>
        /// <returns></returns>
        public IOptionBuilder DefaultShortName()
        {
            return ShortName(null);
        }

        /// <summary>
        /// Sets option description to default (no description).
        /// </summary>
        /// <returns></returns>
        public IOptionBuilder DefaultDescription()
        {
            return Description(null);
        }

        /// <summary>
        /// Sets option name.
        /// When null is passed a value will be set to default (kebab-cased property name).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IOptionBuilder Name(string? name);

        /// <summary>
        /// Sets option short name.
        /// When null is passed a value will be set to default (no short name).
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns></returns>
        IOptionBuilder ShortName(char? shortName);

        /// <summary>
        /// Sets whether option is required.
        /// </summary>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        IOptionBuilder IsRequired(bool isRequired = true);

        /// <summary>
        /// Sets option description.
        /// When null is passed a value will be set to default (no description).
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        IOptionBuilder Description(string? description);

        /// <summary>
        /// Sets an option converter.
        /// </summary>
        /// <typeparam name="TConverter"></typeparam>
        /// <returns></returns>
        IOptionBuilder Converter<TConverter>()
            where TConverter : IArgumentConverter;

        /// <summary>
        /// Sets an option converter.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IOptionBuilder Converter(Type? type);
    }
}