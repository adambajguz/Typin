namespace Typin.Schemas
{
    using System;
    using System.Collections.Generic;
    using Typin.Models.Binding;
    using Typin.Models.Schemas;

    /// <summary>
    /// A command template/blueprint schema.
    /// </summary>
    public sealed class CommandTemplateSchema : ModelSchema, ICommandTemplateSchema
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CommandTemplateSchema"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <param name="options"></param>
        public CommandTemplateSchema(Type type,
                                     IReadOnlyList<IParameterSchema> parameters,
                                     IReadOnlyList<IOptionSchema> options) :
            base(type, parameters, options)
        {

        }
    }
}