namespace BlazorWorker.WorkerBackgroundService
{
    using System.Linq.Expressions;
    using Serialize.Linq.Serializers;

    public class SerializeLinqExpressionSerializer : IExpressionSerializer
    {
        private readonly ExpressionSerializer serializer;

        public SerializeLinqExpressionSerializer()
        {
            serializer = new ExpressionSerializer(new JsonSerializer());
        }

        public Expression Deserialize(string expressionString)
        {
            return serializer.DeserializeText(expressionString);
        }

        public string Serialize(Expression expression)
        {
            return serializer.SerializeText(expression);
        }
    }
}
