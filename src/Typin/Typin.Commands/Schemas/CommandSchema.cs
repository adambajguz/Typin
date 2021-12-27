namespace Typin.Commands.Schemas
{
    using System;
    using Typin.Models.Collections;
    using Typin.Models.Schemas;

    /// <summary>
    /// Command schema.
    /// </summary>
    public class CommandSchema : ICommandSchema
    {
        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public bool IsDefault { get; }

        /// <inheritdoc/>
        public bool IsDynamic { get; }

        /// <inheritdoc/>
        public string? Description { get; }

        /// <inheritdoc/>
        public IModelSchema Model { get; }

        /// <inheritdoc/>
        public Type Handler { get; }

        /// <inheritdoc/>
        public IExtensionsCollection Extensions { get; }

        /// <summary>
        /// Initializes an instance of <see cref="CommandSchema"/>.
        /// </summary>
        public CommandSchema(bool isDynamic,
                             string name,
                             string? description,
                             IModelSchema model,
                             Type handler,
                             IExtensionsCollection extensions)
        {
            Name = name.Trim() ?? throw new ArgumentNullException(nameof(name));
            IsDefault = string.IsNullOrEmpty(Name);
            IsDynamic = isDynamic;
            Description = description;
            Model = model;
            Handler = handler;
            Extensions = extensions;
        }

        ///// <inheritdoc/>
        //public bool CanBeExecutedInMode<T>()
        //    where T : ICliMode
        //{
        //    return CanBeExecutedInMode(typeof(T));
        //}

        ///// <inheritdoc/>
        //public bool CanBeExecutedInMode(Type type)
        //{
        //    if (!KnownTypesHelpers.IsCliModeType(type))
        //    {
        //        throw AttributesExceptions.InvalidModeType(type);
        //    }

        //    if (!HasModeRestrictions())
        //    {
        //        return true;
        //    }

        //    if (SupportedModes is not null && !SupportedModes!.Contains(type))
        //    {
        //        return false;
        //    }

        //    if (ExcludedModes is not null && ExcludedModes.Contains(type))
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        ///// <inheritdoc/>
        //public bool HasModeRestrictions()
        //{
        //    return (SupportedModes?.Count ?? 0) > 0 || (ExcludedModes?.Count ?? 0) > 0;
        //}

        /// <inheritdoc/>
        public override string ToString()
        {
            return base.ToString() +
                " | " +
                $"{nameof(Model)}.{nameof(IModelSchema.Type)} = {Model.Type}, " +
                $"{nameof(Handler)} = {Handler}, " +
                $"{nameof(Name)} = {Name}, " +
                $"{nameof(IsDefault)} = {IsDefault}";
        }
    }
}