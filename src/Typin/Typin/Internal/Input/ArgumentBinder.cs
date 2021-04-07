namespace Typin.Internal.Input
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Typin.Binding;
    using Typin.Internal.Exceptions;
    using Typin.Internal.Extensions;
    using Typin.Schemas;
    using Typin.Utilities;

    internal static class ArgumentBinder
    {
        private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

        private static readonly IReadOnlyDictionary<Type, Func<string, object?>> PrimitiveConverters =
            new Dictionary<Type, Func<string, object?>>
            {
                [typeof(sbyte)] = v => sbyte.Parse(v, FormatProvider),
                [typeof(byte)] = v => byte.Parse(v, FormatProvider),
                [typeof(short)] = v => short.Parse(v, FormatProvider),
                [typeof(ushort)] = v => ushort.Parse(v, FormatProvider),
                [typeof(int)] = v => int.Parse(v, FormatProvider),
                [typeof(uint)] = v => uint.Parse(v, FormatProvider),
                [typeof(long)] = v => long.Parse(v, FormatProvider),
                [typeof(ulong)] = v => ulong.Parse(v, FormatProvider),
#if NET5_0
                [typeof(Half)] = v => Half.Parse(v, FormatProvider),
#endif
                [typeof(float)] = v => float.Parse(v, FormatProvider),
                [typeof(double)] = v => double.Parse(v, FormatProvider),
                [typeof(decimal)] = v => decimal.Parse(v, FormatProvider),
                [typeof(DateTime)] = v => DateTime.Parse(v, FormatProvider),
                [typeof(DateTimeOffset)] = v => DateTimeOffset.Parse(v, FormatProvider),
                [typeof(TimeSpan)] = v => TimeSpan.Parse(v, FormatProvider),
            };

        #region Value Converter
        private static object? ConvertScalar(this ArgumentSchema argumentSchema, string? value, Type targetType)
        {
            try
            {
                // User-defiend type converter
                if (argumentSchema.ConverterType is Type ct)
                {
                    IBindingConverter converterInstance = BindingConverterActivator.GetConverter(ct);

                    return converterInstance.Convert(value);
                }

                // No conversion necessary
                if (targetType == typeof(object) || targetType == typeof(string))
                    return value;

                // Bool conversion (special case)
                if (targetType == typeof(bool))
                    return string.IsNullOrWhiteSpace(value) || bool.Parse(value);

                // Char conversion (special case)
                if (targetType == typeof(char))
                    return TextUtils.UnescapeChar(value);

                // Primitive conversion
                Func<string, object?>? primitiveConverter = PrimitiveConverters.GetValueOrDefault(targetType);
                if (primitiveConverter is not null && !string.IsNullOrWhiteSpace(value))
                    return primitiveConverter(value);

                // Enum conversion conversion
                if (targetType.IsEnum && !string.IsNullOrWhiteSpace(value))
                    return Enum.Parse(targetType, value ?? string.Empty, true);

                // Nullable<T> conversion
                Type? nullableUnderlyingType = targetType.TryGetNullableUnderlyingType();
                if (nullableUnderlyingType is not null)
                {
                    return !string.IsNullOrWhiteSpace(value)
                        ? ConvertScalar(argumentSchema, value, nullableUnderlyingType)
                        : null;
                }

                // String-constructible conversion
                ConstructorInfo? stringConstructor = targetType.GetConstructor(new[] { typeof(string) });
                if (stringConstructor is not null)
                    return stringConstructor.Invoke(new object?[] { value });

                // String-parseable (with format provider) conversion
                MethodInfo? parseMethodWithFormatProvider = targetType.TryGetStaticParseMethod(true);
                if (parseMethodWithFormatProvider is not null)
                    return parseMethodWithFormatProvider.Invoke(null, new object[] { value!, FormatProvider });

                // String-parsable (without format provider) conversion
                MethodInfo? parseMethod = targetType.TryGetStaticParseMethod();
                if (parseMethod is not null)
                    return parseMethod.Invoke(null, new object?[] { value });
            }
            catch (Exception ex)
            {
                throw ArgumentBindingExceptions.CannotConvertToType(argumentSchema, value, targetType, ex);
            }

            throw ArgumentBindingExceptions.CannotConvertToType(argumentSchema, value, targetType);
        }

        private static object ConvertNonScalar(this ArgumentSchema argumentSchema, IReadOnlyCollection<string> values, Type targetEnumerableType, Type targetElementType)
        {
            Array array = values.Select(v => ConvertScalar(argumentSchema, v, targetElementType))
                                .ToNonGenericArray(targetElementType);

            Type arrayType = array.GetType();

            // Assignable from an array
            if (targetEnumerableType.IsAssignableFrom(arrayType))
                return array;

            // Constructible from an array
            ConstructorInfo? arrayConstructor = targetEnumerableType.GetConstructor(new[] { arrayType });
            _ = arrayConstructor ?? throw ArgumentBindingExceptions.CannotConvertNonScalar(argumentSchema, values, targetEnumerableType);

            return arrayConstructor.Invoke(new object[] { array });
        }

        private static object? Convert(this ArgumentSchema argumentSchema, IReadOnlyCollection<string> values)
        {
            PropertyInfo? property = argumentSchema.Property;

            // Short-circuit built-in arguments
            if (property is null)
                return null;

            Type targetType = property.PropertyType;
            Type? enumerableUnderlyingType = property.TryGetEnumerableArgumentUnderlyingType();

            // Scalar
            if (enumerableUnderlyingType is null)
            {
                return values.Count <= 1
                    ? ConvertScalar(argumentSchema, values.SingleOrDefault(), targetType)
                    : throw ArgumentBindingExceptions.CannotConvertMultipleValuesToNonScalar(argumentSchema, values);
            }
            // Non-scalar
            else
            {
                return ConvertNonScalar(argumentSchema, values, targetType, enumerableUnderlyingType);
            }
        }
        #endregion

        /// <summary>
        /// Binds input values to command.
        /// </summary>
        public static void BindOn(this ArgumentSchema argumentSchema, ICommand command, IReadOnlyCollection<string> values)
        {
            if (argumentSchema.Property is null)
                return;

            object? value = argumentSchema.Convert(values);
            argumentSchema.Property.SetValue(command, value);
        }

        /// <summary>
        /// Binds input values to command.
        /// </summary>
        public static void BindOn(this ArgumentSchema argumentSchema, ICommand command, string value)
        {
            BindOn(argumentSchema, command, new[] { value });
        }
    }
}
