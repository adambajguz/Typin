namespace TypinExamples.Infrastructure.WebWorkers.Abstractions
{
    public interface ISerializer
    {
        string Serialize(object obj);

        T Deserialize<T>(string json)
            where T : notnull;
    }
}
