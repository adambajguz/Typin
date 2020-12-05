namespace TypinExamples.Services.Terminal
{
    using System.Threading.Tasks;
    using Microsoft.JSInterop;
    using TypinExamples.Application.Configuration;
    using TypinExamples.Application.Handlers.Commands;
    using TypinExamples.Application.Services;
    using TypinExamples.Infrastructure.WebWorkers.Abstractions;

    public class WebTerminal : IWebTerminal
    {
        public string Id { get; }

        private readonly IJSRuntime _jsRuntime;
        private readonly IWorker _worker;
        private readonly ExampleDescriptor _exampleDescriptor;

        public const string JS_MODULE_NAME = "xtermInterop";

        public WebTerminal(string id, ExampleDescriptor exampleDescriptor, IJSRuntime jsRuntime, IWorker worker)
        {
            Id = id;
            _jsRuntime = jsRuntime;
            _worker = worker;
            _exampleDescriptor = exampleDescriptor;
        }

        public async Task InitializeXtermAsync()
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.initialize", Id);
        }

        public async Task DisposeXtermAsync()
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.dispose", Id);
        }

        public async Task RunExample(string args)
        {
            RunExampleResult result = await _worker.CallCommandAsync<RunExampleCommand, RunExampleResult>(new RunExampleCommand
            {
                Key = _exampleDescriptor.Key,
                Args = args,
                TerminalId = Id,
                ProgramClass = _exampleDescriptor.ProgramClass,
                WebProgramClass = _exampleDescriptor.WebProgramClass
            });
        }

        public async Task ResetAsync()
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.reset", Id);
        }

        public async Task ClearAsync()
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.clear", Id);
        }

        public async Task FocusAsync()
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.focus", Id);
        }

        public async Task BlurAsync()
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.blur", Id);
        }

        public async Task<int> GetRowsCountAsync()
        {
            return await _jsRuntime.InvokeAsync<int>($"{JS_MODULE_NAME}.getRows", Id);
        }

        public async Task<int> GetColumnsCountAsync()
        {
            return await _jsRuntime.InvokeAsync<int>($"{JS_MODULE_NAME}.getColumns", Id);
        }

        public async Task WriteAsync(string str)
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.write", Id, str);
        }

        public async Task WriteLineAsync(string str)
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.writeLine", Id, str);
        }

        public async Task WriteAsync(byte[] buffer)
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.write", Id, buffer);
        }

        public async Task WriteLineAsync(byte[] buffer)
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.writeLine", Id, buffer);
        }

        public async Task ScrollLinesAsync(int lines)
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.scrollLines", Id, lines);
        }

        public async Task ScrollPagesAsync(int pagesCount)
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.scrollPages", Id, pagesCount);
        }

        public async Task ScrollToBottomAsync()
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.scrollToBottom", Id);
        }

        public async Task ScrollToTopAsync()
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.scrollToTop", Id);
        }

        public async Task ScrollToLineAsync(int lineNumber)
        {
            await _jsRuntime.InvokeVoidAsync($"{JS_MODULE_NAME}.scrollToLine", Id, lineNumber);
        }
    }
}
