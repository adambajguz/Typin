namespace Typin.Features.Binder
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Typin.Exceptions.ArgumentBinding;
    using Typin.Models;
    using Typin.Models.Binding;
    using Typin.Models.Converters;
    using Typin.Models.Schemas;
    using Typin.Utilities;
    using Typin.Utilities.Extensions;

    internal static class ArgumentConverter
    {
        private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

        private static object? PrimitiveConverter(Type targetType, string value)
        {
            if (targetType.IsPrimitive)
            {
                if (targetType == typeof(sbyte))
                {
                    return sbyte.Parse(value, FormatProvider);
                }

                if (targetType == typeof(byte))
                {
                    return byte.Parse(value, FormatProvider);
                }

                if (targetType == typeof(short))
                {
                    return short.Parse(value, FormatProvider);
                }

                if (targetType == typeof(ushort))
                {
                    return ushort.Parse(value, FormatProvider);
                }

                if (targetType == typeof(int))
                {
                    return int.Parse(value, FormatProvider);
                }

                if (targetType == typeof(uint))
                {
                    return uint.Parse(value, FormatProvider);
                }

                if (targetType == typeof(long))
                {
                    return long.Parse(value, FormatProvider);
                }

                if (targetType == typeof(ulong))
                {
                    return ulong.Parse(value, FormatProvider);
                }

#if NET5_0_OR_GREATER
                if (targetType == typeof(Half))
                {
                    return Half.Parse(value, FormatProvider);
                }
#endif

                if (targetType == typeof(float))
                {
                    return float.Parse(value, FormatProvider);
                }

                if (targetType == typeof(double))
                {
                    return double.Parse(value, FormatProvider);
                }

                if (targetType == typeof(decimal))
                {
                    return decimal.Parse(value, FormatProvider);
                }
            }

            if (targetType == typeof(Guid))
            {
                return Guid.Parse(value);
            }

            if (targetType == typeof(TimeSpan))
            {
                return TimeSpan.Parse(value, FormatProvider);
            }

            if (targetType == typeof(DateTime))
            {
                return DateTime.Parse(value, FormatProvider);
            }

            if (targetType == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse(value, FormatProvider);
            }

#if NET6_0_OR_GREATER
            if (targetType == typeof(DateOnly))
            {
                return DateOnly.Parse(value, FormatProvider);
            }

            if (targetType == typeof(TimeOnly))
            {
                return TimeOnly.Parse(value, FormatProvider);
            }
#endif

            return null;
        }

        #region Value Converter
        private static object? ConvertScalar(this IArgumentSchema argumentSchema, string? value, Type targetType, IArgumentConverter? converterInstance)
        {
            try
            {
                if (converterInstance is not null && targetType.IsAssignableFrom(converterInstance.TargetType))
                {
                    // Structs do not support inheritance, so we want to execute our Nullable<T> handling logic if converter is for T, not Nullable<T>.
                    // If value is null or whitespace, we must check if this is a nullable struct and converter is for non-nullable struct else normal conversion with converter.
                    if (string.IsNullOrWhiteSpace(value) &&
                        converterInstance.TargetType != targetType &&
                        targetType.TryGetNullableUnderlyingType() is not null)
                    {
                        return null;
                    }

                    return converterInstance.Convert(value);
                }

                // No conversion necessary
                if (targetType == typeof(object) || targetType == typeof(string))
                {
                    return value;
                }

                // Bool conversion (special case)
                if (targetType == typeof(bool))
                {
                    return string.IsNullOrWhiteSpace(value) || bool.Parse(value);
                }

                // Char conversion (special case)
                if (targetType == typeof(char))
                {
                    return TextUtils.UnescapeChar(value);
                }

                // Primitive conversion
                if (!string.IsNullOrWhiteSpace(value) && PrimitiveConverter(targetType, value) is object v)
                {
                    return v;
                }

                // Enum conversion conversion
                if (targetType.IsEnum && !string.IsNullOrWhiteSpace(value))
                {
                    return Enum.Parse(targetType, value ?? string.Empty, true);
                }

                // Nullable<T> conversion
                Type? nullableUnderlyingType = targetType.TryGetNullableUnderlyingType();
                if (nullableUnderlyingType is not null)
                {
                    return !string.IsNullOrWhiteSpace(value)
                        ? argumentSchema.ConvertScalar(value, nullableUnderlyingType, converterInstance: null) // we can pass null because we have an extra check in the beginning of the method
                        : null;
                }

                // String-constructible conversion
                ConstructorInfo? stringConstructor = targetType.GetConstructor(new[] { typeof(string) });
                if (stringConstructor is not null)
                {
                    return stringConstructor.Invoke(new object?[] { value });
                }

                // String-parsable (with format provider) conversion
                MethodInfo? parseMethodWithFormatProvider = targetType.TryGetStaticParseMethod(true);
                if (parseMethodWithFormatProvider is not null)
                {
                    return parseMethodWithFormatProvider.Invoke(null, new object[] { value!, FormatProvider });
                }

                // String-parsable (without format provider) conversion
                MethodInfo? parseMethod = targetType.TryGetStaticParseMethod();
                if (parseMethod is not null)
                {
                    return parseMethod.Invoke(null, new object?[] { value });
                }
            }
            catch (Exception ex)
            {
                throw new TypeConversionException(argumentSchema, value, targetType, ex);
            }

            throw new TypeConversionException(argumentSchema, value, targetType);
        }

        private static object ConvertNonScalar(this IArgumentSchema argumentSchema, IReadOnlyCollection<string> values, Type targetEnumerableType, Type targetElementType, IArgumentConverter? converterInstance)
        {
            Array array = values.Select(v => argumentSchema.ConvertScalar(v, targetElementType, converterInstance))
                                .ToNonGenericArray(targetElementType);

            Type arrayType = array.GetType();

            // Assignable from an array
            if (targetEnumerableType.IsAssignableFrom(arrayType))
            {
                return array;
            }

            // Constructible from an array
            ConstructorInfo? arrayConstructor = targetEnumerableType.GetConstructor(new[] { arrayType });
            _ = arrayConstructor ?? throw new NonScalarNonConstructibleFromArrayException(argumentSchema, values, targetEnumerableType);

            return arrayConstructor.Invoke(new object[] { array });
        }

        private static object? Convert(this IArgumentSchema argumentSchema, IServiceProvider serviceProvider, IReadOnlyCollection<string> values)
        {
            IBindableArgument bindable = argumentSchema.Bindable;

            Type targetType = bindable.Type;
            Type? enumerableUnderlyingType = bindable.EnumerableUnderlyingType;

            // User-defined conversion
            if (argumentSchema.ConverterType is Type converterType)
            {
                IArgumentConverter converterInstance = (IArgumentConverter)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, converterType);

                // Scalar
                if (enumerableUnderlyingType is null)
                {
                    return values.Count <= 1
                        ? argumentSchema.ConvertScalar(values.SingleOrDefault(), targetType, converterInstance)
                        : throw new NonScalarInputExpectedException(argumentSchema, values);
                }
                // Non-scalar with conversion for collection
                else if (targetType.IsAssignableFrom(converterInstance.TargetType))
                {
                    try
                    {
                        return converterInstance.Convert(values);
                    }
                    catch (Exception ex)
                    {
                        throw new TypeConversionException(argumentSchema, values, targetType, ex);
                    }
                }
                // Non-scalar with conversion for collection item type
                else
                {
                    return argumentSchema.ConvertNonScalar(values, targetType, enumerableUnderlyingType, converterInstance);
                }
            }

            // Default conversion
            // Scalar
            if (enumerableUnderlyingType is null)
            {
                return values.Count <= 1
                    ? argumentSchema.ConvertScalar(values.SingleOrDefault(), targetType, null)
                    : throw new NonScalarInputExpectedException(argumentSchema, values);
            }
            // Non-scalar with conversion for collection item type
            else
            {
                return argumentSchema.ConvertNonScalar(values, targetType, enumerableUnderlyingType, null); ;
            }
        }
        #endregion

        /// <summary>
        /// Binds input values to a bindbale object instance.
        /// </summary>
        public static void BindOn(this IArgumentSchema argumentSchema, IServiceProvider serviceProvider, IModel instance, IReadOnlyCollection<string> values)
        {
            object? value = argumentSchema.Convert(serviceProvider, values);
            argumentSchema.Bindable.SetValue(instance, value);
        }

        /// <summary>
        /// Binds input values to a bindbale object instance.
        /// </summary>
        public static void BindOn(this IArgumentSchema argumentSchema, IServiceProvider serviceProvider, IModel instance, string value)
        {
            argumentSchema.BindOn(serviceProvider, instance, new[] { value });
        }
    }
}
