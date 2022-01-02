namespace Typin.Models.Attributes
{
    using System.Reflection;
    using Typin.Models;
    using Typin.Models.Builders;

    /// <summary>
    /// Command builder attributes configuration extensions.
    /// </summary>
    public static class CommandBuilderAttributesExtensions
    {
        /// <summary>
        /// Configures the model from attributes.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IModelBuilder<TCommand> FromAttributes<TCommand>(this IModelBuilder<TCommand> builder)
            where TCommand : class, IModel
        {
            PropertyInfo[] modelProperties = builder.Model.GetProperties();

            for (int i = 0; i < modelProperties.Length; i++)
            {
                PropertyInfo? property = modelProperties[i];

                if (property.GetCustomAttribute<ParameterAttribute>() is ParameterAttribute parameterAttribute)
                {
                    builder.Parameter(property)
                        .Name(parameterAttribute.Name)
                        .Description(parameterAttribute.Description)
                        .Converter(parameterAttribute.Converter);
                }

                if (property.GetCustomAttribute<OptionAttribute>() is OptionAttribute optionAttribute)
                {
                    builder.Option(property)
                        .Name(optionAttribute.Name)
                        .ShortName(optionAttribute.ShortName)
                        .IsRequired(optionAttribute.IsRequired)
                        .Description(optionAttribute.Description)
                        .Converter(optionAttribute.Converter);
                }
            }

            return builder;
        }

        /// <summary>
        /// Configures the model from attributes.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IModelBuilder FromAttributes(this IModelBuilder builder)
        {
            PropertyInfo[] modelProperties = builder.Model.GetProperties();

            for (int i = 0; i < modelProperties.Length; i++)
            {
                PropertyInfo? property = modelProperties[i];

                if (property.GetCustomAttribute<ParameterAttribute>() is ParameterAttribute parameterAttribute)
                {
                    builder.Parameter(property)
                        .Name(parameterAttribute.Name)
                        .Description(parameterAttribute.Description)
                        .Converter(parameterAttribute.Converter);
                }

                if (property.GetCustomAttribute<OptionAttribute>() is OptionAttribute optionAttribute)
                {
                    builder.Option(property)
                        .Name(optionAttribute.Name)
                        .ShortName(optionAttribute.ShortName)
                        .IsRequired(optionAttribute.IsRequired)
                        .Description(optionAttribute.Description)
                        .Converter(optionAttribute.Converter);
                }
            }

            return builder;
        }
    }
}
