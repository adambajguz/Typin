namespace Typin.Internal.Input
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Typin.Internal.Exceptions;
    using Typin.Internal.Extensions;
    using Typin.Schemas;

    internal static class ArgumentBinder
    {
        private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

        private static readonly IReadOnlyDictionary<Type, Func<string?, object?>> PrimitiveConverters =
            new Dictionary<Type, Func<string?, object?>>
            {
                [typeof(object)] = v => v,
                [typeof(string)] = v => v,
                [typeof(bool)] = v => string.IsNullOrWhiteSpace(v) || bool.Parse(v),
                [typeof(char)] = v => v!.Single(),
                [typeof(sbyte)] = v => sbyte.Parse(v!, FormatProvider),
                [typeof(byte)] = v => byte.Parse(v!, FormatProvider),
                [typeof(short)] = v => short.Parse(v!, FormatProvider),
                [typeof(ushort)] = v => ushort.Parse(v!, FormatProvider),
                [typeof(int)] = v => int.Parse(v!, FormatProvider),
                [typeof(uint)] = v => uint.Parse(v!, FormatProvider),
                [typeof(long)] = v => long.Parse(v!, FormatProvider),
                [typeof(ulong)] = v => ulong.Parse(v!, FormatProvider),
#if NET5_0
                [typeof(Half)] = v => Half.Parse(v!, FormatProvider),
#endif
                [typeof(float)] = v => float.Parse(v!, FormatProvider),
                [typeof(double)] = v => double.Parse(v!, FormatProvider),
                [typeof(decimal)] = v => decimal.Parse(v!, FormatProvider),
                [typeof(DateTime)] = v => DateTime.Parse(v!, FormatProvider),
                [typeof(DateTimeOffset)] = v => DateTimeOffset.Parse(v!, FormatProvider),
                [typeof(TimeSpan)] = v => TimeSpan.Parse(v!, FormatProvider),
            };

        #region Value Converter
        private static object? ConvertScalar(this ArgumentSchema argumentSchema, string? value, Type targetType)
        {
            try
            {
                // Primitive
                Func<string?, object?>? primitiveConverter = PrimitiveConverters.GetValueOrDefault(targetType);
                if (primitiveConverter != null)
                    return primitiveConverter(value);

                // Enum
                if (targetType.IsEnum)
                    return Enum.Parse(targetType, value ?? string.Empty, true);

                // Nullable
                Type? nullableUnderlyingType = targetType.TryGetNullableUnderlyingType();
                if (nullableUnderlyingType != null)
                {
                    return !string.IsNullOrWhiteSpace(value)
                        ? ConvertScalar(argumentSchema, value, nullableUnderlyingType)
                        : null;
                }

                // String-constructible
                ConstructorInfo? stringConstructor = targetType.GetConstructor(new[] { typeof(string) });
                if (stringConstructor != null)
                    return stringConstructor.Invoke(new object?[] { value });

                // String-parseable (with format provider)
                MethodInfo? parseMethodWithFormatProvider = targetType.GetStaticParseMethod(true);
                if (parseMethodWithFormatProvider != null)
                    return parseMethodWithFormatProvider.Invoke(null, new object[] { value!, FormatProvider });

                // String-parseable (without format provider)
                MethodInfo? parseMethod = targetType.GetStaticParseMethod();
                if (parseMethod != null)
                    return parseMethod.Invoke(null, new object?[] { value });
            }
            catch (Exception ex)
            {
                throw ArgumentBindingExceptions.CannotConvertToType(argumentSchema, value, targetType, ex);
            }

            throw ArgumentBindingExceptions.CannotConvertToType(argumentSchema, value, targetType);
        }

        private static object ConvertNonScalar(this ArgumentSchema argumentSchema, IReadOnlyList<string> values, Type targetEnumerableType, Type targetElementType)
        {
            Array array = values.Select(v => ConvertScalar(argumentSchema, v, targetElementType))
                                .ToNonGenericArray(targetElementType);

            Type arrayType = array.GetType();

            // Assignable from an array
            if (targetEnumerableType.IsAssignableFrom(arrayType))
                return array;

            // Constructible from an array
            ConstructorInfo? arrayConstructor = targetEnumerableType.GetConstructor(new[] { arrayType });
            if (arrayConstructor != null)
                return arrayConstructor.Invoke(new object[] { array });

            throw ArgumentBindingExceptions.CannotConvertNonScalar(argumentSchema, values, targetEnumerableType);
        }

        private static object? Convert(this ArgumentSchema argumentSchema, IReadOnlyList<string> values)
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
        public static void BindOn(this ArgumentSchema argumentSchema, ICommand command, IReadOnlyList<string> values)
        {
            if (argumentSchema.Property is null)
                return;

            object? value = argumentSchema.Convert(values);
            argumentSchema.Property.SetValue(command, value);
        }

        /// <summary>
        /// Binds input values to command.
        /// </summary>
        public static void BindOn(this ArgumentSchema argumentSchema, ICommand command, params string[] values)
        {
            BindOn(argumentSchema, command, (IReadOnlyList<string>)values);
        }
    }
}
