namespace Typin.Schemas.Resolvers
{
    internal interface IResolver<out T>
        where T : class
    {
        bool IsResolved { get; }
        T Resolve();
    }
}