namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    public interface ISerializer
    {
        string Serialize(object obj);

        T Deserialize<T>(string objStr);
    }
}
