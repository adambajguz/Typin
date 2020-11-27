namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal.JS
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.JSInterop;

    internal class ScriptLoader
    {
        public const string MODULE_NAME = "BlazorWebWorker";
        private const string JS_FILE = "BlazorWebWorker.js";

        private static readonly IReadOnlyDictionary<string, string> escapeScriptTextReplacements =
            new Dictionary<string, string> { { @"\", @"\\" }, { "\r", @"\r" }, { "\n", @"\n" }, { "'", @"\'" }, { "\"", @"\""" } };

        private readonly IJSRuntime jsRuntime;

        public ScriptLoader(IJSRuntime jSRuntime)
        {
            jsRuntime = jSRuntime;
        }

        public async Task InitScript()
        {
            if (await IsLoaded())
                return;

            Assembly assembly = typeof(ScriptLoader).Assembly;
            string assemblyName = assembly.GetName().Name ?? throw new NullReferenceException($"Unable to initialize {JS_FILE}");

            using (Stream stream = assembly.GetManifestResourceStream($"{assemblyName}.{JS_FILE}") ?? throw new InvalidOperationException($"Unable to get {JS_FILE}"))
            using (StreamReader streamReader = new StreamReader(stream))
            {
                string scriptContent = await streamReader.ReadToEndAsync();

                await ExecuteRawScriptAsync(scriptContent);
            }

            DateTimeOffset startedOn = DateTimeOffset.UtcNow;
            DateTimeOffset shouldFinishOn = startedOn.AddSeconds(4);
            while (!await IsLoaded())
            {
                await Task.Delay(100);

                // Fail after 4s not to block and hide any other possible error
                if (DateTimeOffset.UtcNow > shouldFinishOn)
                    throw new InvalidOperationException($"Unable to initialize {JS_FILE}. Operation started on {startedOn} and was expected to finish {shouldFinishOn}.");
            }
        }

        private async Task<bool> IsLoaded()
        {
            return await jsRuntime.InvokeAsync<bool>("window.hasOwnProperty", MODULE_NAME);
        }

        private async Task ExecuteRawScriptAsync(string scriptContent)
        {
            scriptContent = escapeScriptTextReplacements.Aggregate(scriptContent, (r, pair) => r.Replace(pair.Key, pair.Value));

            StringBuilder builder = new();
            builder.Append("(function(){var d = document; var s = d.createElement('script'); s.async=false; s.src=");
            builder.Append("URL.createObjectURL(new Blob([\"");
            builder.Append(scriptContent);
            builder.Append("\"],{ \"type\": \"text/javascript\"})); d.head.appendChild(s); d.head.removeChild(s);})();");

            string script = builder.ToString();

            await jsRuntime.InvokeVoidAsync("eval", script);
        }
    }
}
