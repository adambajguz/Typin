namespace Typin.Models.Builders
{
    using System;
    using System.Linq.Expressions;
    using Typin.Models.Converters;
    using Typin.Models.Extensions;
    using Typin.Schemas.Collections;

    /// <summary>
    /// Option builder.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    internal class OptionBuilder<TModel, TProperty> : OptionBuilder<TModel>, IOptionBuilder<TModel, TProperty>
        where TModel : class, IModel
    {
        /// <summary>
        /// Initializes a new instance of <see cref="OptionBuilder{TModel, TProperty}"/>.
        /// </summary>
        /// <param name="propertyAccessor"></param>
        public OptionBuilder(Expression<Func<TModel, TProperty>> propertyAccessor) :
            base(propertyAccessor.GetPropertyInfo())
        {

        }

        IOptionBuilder<TModel, TProperty> IManageExtensions<IOptionBuilder<TModel, TProperty>>.ManageExtensions(Action<IExtensionsCollection> action)
        {
            action(Extensions);

            return this;
        }

        IOptionBuilder<TModel, TProperty> IOptionBuilder<TModel, TProperty>.Name(string? name)
        {
            Name(name);

            return this;
        }

        IOptionBuilder<TModel, TProperty> IOptionBuilder<TModel, TProperty>.ShortName(char? shortName)
        {
            ShortName(shortName);

            return this;
        }

        IOptionBuilder<TModel, TProperty> IOptionBuilder<TModel, TProperty>.IsRequired(bool isRequired)
        {
            IsRequired(isRequired);

            return this;
        }

        IOptionBuilder<TModel, TProperty> IOptionBuilder<TModel, TProperty>.Description(string? description)
        {
            Description(description);

            return this;
        }

        IOptionBuilder<TModel, TProperty> IOptionBuilder<TModel, TProperty>.Converter<TConverter>()
        {
            ConverterType = typeof(TConverter);

            return this;
        }

        IOptionBuilder<TModel, TProperty> IOptionBuilder<TModel, TProperty>.Converter(Type? type)
        {
            if (type is not null && !IArgumentConverter<TProperty>.IsValidType(type))
            {
                throw new ArgumentException($"'{type}' is not a valid converter for option property of type '{typeof(TProperty)}' inside model '{typeof(TModel)}'.");
            }

            ConverterType = type;

            return this;
        }
    }
}
