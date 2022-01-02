namespace TypinExamples.Shared.Components
{
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;
    using TypinExamples.Services;

    public partial class FileBrowser : ComponentBase
    {
        private const string EditorId = "m-filebrowser--monaco-container";

        private bool IsInitialized { get; set; }

        [Inject] private ILogger<FileBrowser> Logger { get; init; } = default!;
        [Inject] private MonacoEditorService Editor { get; init; } = default!;
        [Inject] private HttpClient HttpClient { get; init; } = default!;

        [Parameter]
        public string? Root { get; init; }

        [Parameter]
        public string[]? SrcFiles { get; init; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await Editor.InitializeAsync(EditorId, string.Empty, "csharp", "vs-dark", true, true);

                await Task.Delay(1100);
                IsInitialized = true;
                StateHasChanged();

                await ChangeFile(SrcFiles?.First() ?? string.Empty);
            }
        }

        private async Task ToggleLineAsync()
        {
            await Editor.ToggleLineNumbersVisibility(EditorId);
        }

        private async Task ChangeFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(Root) || string.IsNullOrWhiteSpace(filename))
            {
                Logger.LogError($"Invalid download file - {nameof(Root)} {{Root}} or {nameof(filename)} {{FileName}} not set.", Root, filename);
                return;
            }

            string requestUri = Path.Combine(Root, filename);
            HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string sourceCode = await response.Content.ReadAsStringAsync();
                await Editor.SetTextAsync(EditorId, sourceCode);
            }
            else
            {
                Logger.LogError("Failed to fetch file {File}.", requestUri);
            }
        }
    }
}
