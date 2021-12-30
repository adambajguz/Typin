namespace Typin.Directives
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq.Expressions;
    using System.Reflection;
    using Typin;
    using Typin.Directives.Internal;

    /// <summary>
    /// <see cref="IDirectiveArgs{TDirective}"/> factory.
    /// </summary>
    public static class DirectiveArgsFactory
    {
        private static readonly ConcurrentDictionary<Type, Func<IDirective, CliContext, IDirectiveArgs>> _instanceFactoryCache = new();

        /// <summary>
        /// Creates a directive arguments instance.
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDirectiveArgs<TDirective> Create<TDirective>(TDirective directive, CliContext context)
            where TDirective : class, IDirective
        {
            static IDirectiveArgs factory(IDirective directive, CliContext context)
            {
                return new DirectiveArgs<TDirective>(directive, context);
            }

            _instanceFactoryCache.GetOrAdd(typeof(TDirective), _ => factory);

            return new DirectiveArgs<TDirective>(directive, context);
        }

        /// <summary>
        /// Creates a directive arguments instance.
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDirectiveArgs Create(IDirective directive, CliContext context)
        {
            var factory = _instanceFactoryCache.GetOrAdd(directive.GetType(), NewInstance);
            IDirectiveArgs args = factory(directive, context);

            return args;
        }

        private static Func<IDirective, CliContext, IDirectiveArgs> NewInstance(Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            var constructor = typeof(DirectiveArgs<>).MakeGenericType(type).GetConstructors(bindingFlags)[1];

            var parameter0 = Expression.Parameter(typeof(IDirective), "directive");
            var parameter1 = Expression.Parameter(typeof(CliContext), "context");
            var constructorExpression = Expression.New(constructor, parameter0, parameter1);
            var lambdaExpression = Expression.Lambda<Func<IDirective, CliContext, IDirectiveArgs>>(constructorExpression, parameter0, parameter1);

            return lambdaExpression.Compile();
        }
    }
}
