namespace Typin.Models.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Expression extensions.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Get member info from expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Throws when expression type is not supported.</exception>
        public static MemberInfo GetMemberInfo<T>(this Expression<T> expression)
        {
            return expression.Body switch
            {
                MemberExpression m => m.Member,
                UnaryExpression u when u.Operand is MemberExpression m => m.Member,
                _ => throw new InvalidOperationException(expression.GetType().ToString())
            };
        }

        /// <summary>
        /// Get property info from expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Throws when expression type is not supported.</exception>
        public static PropertyInfo GetPropertyInfo<T>(this Expression<T> expression)
        {
            return expression.Body switch
            {
                MemberExpression m => (PropertyInfo)m.Member,
                UnaryExpression u when u.Operand is MemberExpression m => (PropertyInfo)m.Member,
                _ => throw new InvalidOperationException(expression.GetType().ToString())
            };
        }
    }
}
