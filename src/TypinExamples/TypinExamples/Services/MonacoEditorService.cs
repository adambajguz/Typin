namespace TypinExamples.Services
{
    using Microsoft.JSInterop;

    public class MonacoEditorService
    {
        private readonly IJSInProcessRuntime Runtime;

        public MonacoEditorService(IJSRuntime runtime)
        {
            Runtime = runtime as IJSInProcessRuntime;
        }

        public void Initialize(string elementId, string initialCode, string language)
        {
            Runtime.Invoke<object>("monacoInterop.initialize", elementId, initialCode, language);
        }

        public string GetCode(string elementId)
        {
            return Runtime.Invoke<string>("monacoInterop.getCode", elementId);
        }

        public void SetCode(string elementId, string code)
        {
            Runtime.Invoke<object>("monacoInterop.setCode", elementId, code);
        }
    }
}
