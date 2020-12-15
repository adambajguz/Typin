namespace TypinExamples.Services
{
    using System.Threading.Tasks;
    using Microsoft.JSInterop;

    public class MonacoEditorService
    {
        private readonly IJSRuntime Runtime;

        public MonacoEditorService(IJSRuntime runtime)
        {
            Runtime = runtime;
        }

        public async Task InitializeAsync(string elementId, string initialCode, string language, string theme, bool readOnly)
        {
            await Runtime.InvokeVoidAsync("monacoInterop.initialize", elementId, initialCode, language, theme, readOnly);
        }

        public async Task<string> GetTextAsync(string elementId)
        {
            return await Runtime.InvokeAsync<string>("monacoInterop.getText", elementId);
        }

        public async Task SetTextAsync(string elementId, string code)
        {
            await Runtime.InvokeVoidAsync("monacoInterop.setText", elementId, code);
        }

        public async Task ShowLineNumbers(string elementId)
        {
            await Runtime.InvokeVoidAsync("monacoInterop.showLineNumbers", elementId);
        }

        public async Task HideLineNumbers(string elementId)
        {
            await Runtime.InvokeVoidAsync("monacoInterop.hideLineNumbers", elementId);
        }

        public async Task ToggleLineNumbersVisibility(string elementId)
        {
            await Runtime.InvokeVoidAsync("monacoInterop.toggleLineNumbersVisibility", elementId);
        }
    }
}
