namespace TypinExamples.Infrastructure.WebWorkers.Core.Internal.JS
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.JSInterop;

    public class ScriptLoader : IScriptLoader
    {
        public const string ModuleName = "BlazorWebWorker";
        private const string JSFileName = "BlazorWebWorker.js";

        private static readonly IReadOnlyDictionary<string, string> escapeScriptTextReplacements =
            new Dictionary<string, string> { { @"\", @"\\" }, { "\r", @"\r" }, { "\n", @"\n" }, { "'", @"\'" }, { "\"", @"\""" } };

        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger _logger;

        public ScriptLoader(IJSRuntime jSRuntime, ILogger<ScriptLoader> logger)
        {
            _jsRuntime = jSRuntime;
            _logger = logger;
        }

        public async Task InitScript()
        {
            if (await IsLoaded())
                return;

            Assembly assembly = typeof(ScriptLoader).Assembly;
            string assemblyName = assembly.GetName().Name ?? throw new NullReferenceException($"Unable to initialize {JSFileName}.");

            using (Stream stream = assembly.GetManifestResourceStream($"{assemblyName}.{JSFileName}") ?? throw new InvalidOperationException($"Unable to get {JSFileName}"))
            using (StreamReader streamReader = new StreamReader(stream))
            {
                string scriptContent = await streamReader.ReadToEndAsync();

                await ExecuteRawScriptAsync(scriptContent);
            }

            // Fail after 4s not to block and hide any other possible error
            using (CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(6)))
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();

                bool isLoaded;
                do
                {
                    isLoaded = await IsLoaded();
                    await Task.Delay(50);
                }
                while (!isLoaded && !cancellationTokenSource.IsCancellationRequested);

                stopwatch.Stop();

                if (isLoaded)
                {
                    _logger.LogDebug($"Initialized {JSFileName} with module {ModuleName} after {{Elapsed}}.", stopwatch.Elapsed);
                }
                else if (cancellationTokenSource.IsCancellationRequested)
                {
                    _logger.LogCritical($"Unable to initialize {JSFileName} with module {ModuleName}. Operation cancelled after about 6 seconds {{Elapsed}}.", stopwatch.Elapsed);
                }
            }
        }

        private async Task<bool> IsLoaded()
        {
            return await _jsRuntime.InvokeAsync<bool>("window.hasOwnProperty", ModuleName);
        }

        private async Task ExecuteRawScriptAsync(string scriptContent)
        {
            scriptContent = escapeScriptTextReplacements.Aggregate(scriptContent, (r, pair) => r.Replace(pair.Key, pair.Value));

            StringBuilder builder = new();
            builder.Append("(function(){let d = document; var s = d.createElement('script'); s.async=false; s.src=");
            builder.Append("URL.createObjectURL(new Blob([\"");
            builder.Append(scriptContent);
            builder.Append("\"],{\"type\": \"text/javascript\"})); d.head.appendChild(s); d.head.removeChild(s);})();");

            string script = builder.ToString();

            await _jsRuntime.InvokeVoidAsync("eval", script);
        }
    }
}
