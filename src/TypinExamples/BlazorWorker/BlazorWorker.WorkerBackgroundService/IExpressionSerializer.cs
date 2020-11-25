namespace BlazorWorker.WorkerBackgroundService
{
    using System.Linq.Expressions;

    public interface IExpressionSerializer
    {
        string Serialize(Expression expr);

        Expression Deserialize(string exprString);
    }
}
