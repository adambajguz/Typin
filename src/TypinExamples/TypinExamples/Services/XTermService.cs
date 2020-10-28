namespace TypinExamples.Services
{
    using Microsoft.JSInterop;

    //https://medium.com/codingtown/xterm-js-terminal-2b19ccd2a52
    public class XTermService
    {
        private readonly IJSInProcessRuntime Runtime;

        public XTermService(IJSRuntime runtime)
        {
            Runtime = runtime as IJSInProcessRuntime;
        }

        public void Initialize(string elementId)
        {
            Runtime.Invoke<object>("xtermInterop.initialize", elementId);
        }

        public string Write(string elementId, string str)
        {
            return Runtime.Invoke<string>("xtermInterop.write", elementId, str);
        }
    }
}
