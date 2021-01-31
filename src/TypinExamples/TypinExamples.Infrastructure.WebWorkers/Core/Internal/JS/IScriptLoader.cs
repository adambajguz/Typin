using System.Threading.Tasks;

namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal.JS
{
    public interface IScriptLoader
    {
        Task InitScript();
    }
}