namespace TypinExamples.Infrastructure.WebWorkers.Hil
{
    public class WebWorkerOptions
    {
        private ISerializer? messageSerializer;

        public ISerializer MessageSerializer
        {
            get => messageSerializer ??= new DefaultMessageSerializer();
            set => messageSerializer = value;
        }
    }
}
