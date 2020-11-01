namespace TypinExamples.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Reflection;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.Extensions.Logging;

    public class BlazorBoot
    {
        [JsonPropertyName("cacheBootResources")]
        public bool CacheBootResources { get; set; }

        [JsonPropertyName("config")]
        public string[] Config { get; set; } = Array.Empty<string>();

        [JsonPropertyName("debugBuild")]
        public bool DebugBuild { get; set; }

        [JsonPropertyName("entryAssembly")]
        public string EntryAssembly { get; set; } = string.Empty;

        [JsonPropertyName("linkerEnabled")]
        public bool LinkerEnabled { get; set; }

        [JsonPropertyName("resources")]
        public Resources Resources { get; set; } = new Resources();
    }

    public class Resources
    {
        [JsonPropertyName("assembly")]
        public Dictionary<string, string> Assembly { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("pdb")]
        public Dictionary<string, string> Pdb { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("runtime")]
        public Dictionary<string, string> Runtime { get; set; } = new Dictionary<string, string>();
    }

    public class RoslynCompilerService
    {
        private Task? InitializationTask { get; set; }
        private List<MetadataReference>? References { get; set; }

        private ILogger<RoslynCompilerService> Logger { get; }

        public RoslynCompilerService(ILogger<RoslynCompilerService> logger)
        {
            Logger = logger;
        }

        public void InitializeMetadataReferences(HttpClient client)
        {
            async Task InitializeInternal()
            {
                Logger.LogInformation("Initializing compiler");
                BlazorBoot response = await client.GetFromJsonAsync<BlazorBoot>("_framework/blazor.boot.json");
                HttpResponseMessage[] assemblies = await Task.WhenAll(response.Resources.Assembly.Keys.Select(x => client.GetAsync("_framework/_bin/" + x)));

                List<MetadataReference> references = new List<MetadataReference>(assemblies.Length);
                foreach (HttpResponseMessage asm in assemblies)
                {
                    using (Stream task = await asm.Content.ReadAsStreamAsync())
                    {
                        references.Add(MetadataReference.CreateFromStream(task));
                    }
                }

                References = references;
                Logger.LogInformation("Finished compiler initilization");
            }
            InitializationTask = InitializeInternal();
        }

        public Task WhenReady(Func<Task> action)
        {
            if (InitializationTask.Status != TaskStatus.RanToCompletion)
            {
                return InitializationTask.ContinueWith(x => action());
            }
            else
            {
                return action();
            }
        }

        public (bool success, Assembly? asm) LoadSource(string source)
        {
            CSharpCompilation compilation = CSharpCompilation.Create("DynamicCode")
                                                             .WithOptions(new CSharpCompilationOptions(OutputKind.ConsoleApplication))
                                                             .AddReferences(References)
                                                             .AddSyntaxTrees(CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview)));

            ImmutableArray<Diagnostic> diagnostics = compilation.GetDiagnostics();

            bool error = false;
            foreach (Diagnostic diag in diagnostics)
            {
                switch (diag.Severity)
                {
                    case DiagnosticSeverity.Info:
                        Console.WriteLine(diag.ToString());
                        break;
                    case DiagnosticSeverity.Warning:
                        Console.WriteLine(diag.ToString());
                        break;
                    case DiagnosticSeverity.Error:
                        error = true;
                        Console.WriteLine(diag.ToString());
                        break;
                }
            }

            if (error)
            {
                return (false, null);
            }

            using (var outputAssembly = new MemoryStream())
            {
                compilation.Emit(outputAssembly);

                return (true, Assembly.Load(outputAssembly.ToArray()));
            }
        }

        public static string Format(string source)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            SyntaxNode root = tree.GetRoot();
            SyntaxNode normalized = root.NormalizeWhitespace();

            return normalized.ToString();
        }
    }
}
