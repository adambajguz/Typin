namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal
{
    internal sealed class IdProvider
    {
        private ulong _id;

        public IdProvider()
        {

        }

        public ulong Next()
        {
            return _id++;
        }
    }
}
