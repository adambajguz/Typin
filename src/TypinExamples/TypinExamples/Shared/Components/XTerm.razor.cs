namespace TypinExamples.Shared.Components
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.Extensions.Logging;
    using Microsoft.JSInterop;
    using TypinExamples.Services.Terminal;
    using TypinExamples.TypinWeb;

    public sealed partial class XTerm : ComponentBase, IWebTerminal, IAsyncDisposable
    {
        private const string MODULE_NAME = "xtermInterop";

        public bool IsDisposed { get; private set; }

        public string Id { get; } = string.Concat("m-", Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture));

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; } = new Dictionary<string, object>();

        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKey { get; set; }

        [Parameter]
        public EventCallback OnLineFeed { get; set; }

        [Parameter]
        public TerminalOptions Options { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private ILogger<XTerm> Logger { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Options == null)
                {
                    Options = new TerminalOptions();
                }

                await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.initialize", Id);
                Logger.LogDebug("Initialized a new XTerm terminal ({Id})", Id);
                //TerminalManager.RegisterTerminal(TerminalId, this);
            }

            await WriteLineAsync("Test");

            WebConsole webConsole = new WebConsole(this);
            await HelloWorld.Program.WebMain(webConsole, new List<string>(), new Dictionary<string, string>());
        }

        public async Task ResetAsync()
        {
            Logger.LogDebug("ResetAsync()");
            await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.reset", Id);
        }

        public async Task ClearAsync()
        {
            Logger.LogDebug("ClearAsync()");
            await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.clear", Id);
        }

        public async Task FocusAsync()
        {
            Logger.LogDebug("FocusAsync()");
            await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.focus", Id);
        }

        public async Task BlurAsync()
        {
            Logger.LogDebug("BlurAsync()");
            await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.blur", Id);
        }

        public async Task<int> GetRowsCountAsync()
        {
            Logger.LogDebug("GetRowsCountAsync()");
            return await JSRuntime.InvokeAsync<int>($"{MODULE_NAME}.getRows", Id);
        }

        public async Task<int> GetColumnsCountAsync()
        {
            Logger.LogDebug("GetColumnsCountAsync()");
            return await JSRuntime.InvokeAsync<int>($"{MODULE_NAME}.getColumns", Id);
        }

        public async Task WriteAsync(string str)
        {
            Logger.LogDebug("WriteAsync(\"{str}\")", str);
            await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.write", Id, str);
        }

        public async Task WriteLineAsync(string str)
        {
            Logger.LogDebug("WriteLineAsync(\"{str}\")", str);
            await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.writeLine", Id, str);
        }

        public async Task ScrollLinesAsync(int lines)
        {
            Logger.LogDebug("ScrollLinesAsync({lines})", lines);
            await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.scrollLines", Id, lines);
        }

        public async Task ScrollPagesAsync(int pagesCount)
        {
            Logger.LogDebug("ScrollPagesAsync({pagesCount})", pagesCount);
            await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.scrollPages", Id, pagesCount);
        }

        public async Task ScrollToBottomAsync()
        {
            Logger.LogDebug("ScrollToBottomAsync()");
            await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.scrollToBottom", Id);
        }

        public async Task ScrollToTopAsync()
        {
            Logger.LogDebug("ScrollToTopAsync()");
            await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.scrollToTop", Id);
        }

        public async Task ScrollToLineAsync(int lineNumber)
        {
            Logger.LogDebug("ScrollToLineAsync({lineNumber})", lineNumber);
            await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.scrollToLine", Id, lineNumber);
        }

        public async ValueTask DisposeAsync()
        {
            IsDisposed = true;
            await JSRuntime.InvokeVoidAsync($"{MODULE_NAME}.initialize", Id);
            Logger.LogDebug("Disposed XTerm terminal ({Id})", Id);
            //TerminalManager.UnregisterTerminal(TerminalId);
        }
    }
}