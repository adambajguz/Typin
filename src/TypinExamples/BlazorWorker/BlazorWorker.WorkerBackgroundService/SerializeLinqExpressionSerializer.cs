using System.Linq.Expressions;
using Serialize.Linq.Serializers;

namespace BlazorWorker.WorkerBackgroundService
{
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
