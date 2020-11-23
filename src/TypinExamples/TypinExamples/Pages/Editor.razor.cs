namespace TypinExamples.Pages
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using TypinExamples.Infrastructure.Compiler.Services;
    using TypinExamples.Services;

    public partial class Editor
    {
        public string Output = "";
        private const string DefaultCode = @"using System;

class Program
{
    public static void Main()
    {
        Console.WriteLine(""Hello World"");
    }
}";

        [Inject] private HttpClient Client { get; set; } = default!;
        [Inject] private MonacoEditorService MonacoEditor { get; set; } = default!;
        [Inject] private RoslynCompilerService Compiler { get; set; } = default!;

        protected override Task OnInitializedAsync()
        {
            Compiler.InitializeMetadataReferences(Client);
            return base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                MonacoEditor.Initialize("container", DefaultCode, "csharp", "vs-dark", false);
                Run();
            }
        }

        public Task Run()
        {
            return Compiler.WhenReady(RunInternal);
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        private async Task RunInternal()
        {
            Output = "";

            Console.WriteLine("Compiling and Running code");
            Stopwatch sw = Stopwatch.StartNew();

            TextWriter currentOut = Console.Out;
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);

            Exception? exception = null;
            try
            {
                (bool success, Assembly asm) = Compiler.LoadSource(MonacoEditor.GetCode("container"));
                if (success)
                {
                    MethodInfo entry = asm.EntryPoint;
                    if (entry.Name == "<Main>") // sync wrapper over async Task Main
                    {
                        entry = entry.DeclaringType.GetMethod("Main", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static); // reflect for the async Task Main
                    }

                    bool hasArgs = entry.GetParameters().Length > 0;
                    object result = entry.Invoke(null, hasArgs ? new object[] { Array.Empty<string>() } : null);
                    if (result is Task t)
                    {
                        await t;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Output = writer.ToString();
            if (exception != null)
            {
                Output += "\r\n" + exception.ToString();
            }
            Console.SetOut(currentOut);

            sw.Stop();
            Console.WriteLine("Done in " + sw.ElapsedMilliseconds + "ms");
        }
    }
}
