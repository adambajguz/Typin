namespace TypinExamples.Application.Services.Compiler
{
    using System;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;

    public interface IRoslynCompilerService
    {
        void InitializeMetadataReferences(HttpClient client);
        (bool success, Assembly? asm) LoadSource(string source);
        Task WhenReady(Func<Task> action);
    }
}