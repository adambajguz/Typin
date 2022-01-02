namespace Typin.Models.Builders
{
    using System;
    using System.Reflection;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Option builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    internal class OptionBuilder<TModel> : OptionBuilder, IOptionBuilder<TModel>
        where TModel : class, IModel
    {
        /// <summary>
        /// Initializes a new instance of <see cref="OptionBuilder{TModel}"/>.
        /// </summary>
        /// <param name="propertyInfo"></param>
        public OptionBuilder(PropertyInfo propertyInfo) :
            base(typeof(TModel), propertyInfo)
        {

        }

        IOptionBuilder<TModel> IManageExtensions<IOptionBuilder<TModel>>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        IOptionBuilder<TModel> IOptionBuilder<TModel>.Name(string? name)
        {
            Name(name);

            return this;
        }

        IOptionBuilder<TModel> IOptionBuilder<TModel>.ShortName(char? shortName)
        {
            ShortName(shortName);

            return this;
        }

        IOptionBuilder<TModel> IOptionBuilder<TModel>.IsRequired(bool isRequired)
        {
            IsRequired(isRequired);

            return this;
        }

        IOptionBuilder<TModel> IOptionBuilder<TModel>.Description(string? description)
        {
            Description(description);

            return this;
        }

        IOptionBuilder<TModel> IOptionBuilder<TModel>.Fallback(string? variableName)
        {
            Fallback(variableName);

            return this;
        }

        IOptionBuilder<TModel> IOptionBuilder<TModel>.Converter<TConverter>()
        {
            Converter(typeof(TConverter));

            return this;
        }

        IOptionBuilder<TModel> IOptionBuilder<TModel>.Converter(Type? type)
        {
            Converter(type);

            return this;
        }
    }
}
