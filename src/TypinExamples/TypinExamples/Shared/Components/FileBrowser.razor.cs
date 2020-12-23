namespace TypinExamples.Shared.Components
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using TypinExamples.Services;

    public partial class FileBrowser : ComponentBase
    {
        private bool IsInitialized { get; set; }

        [Inject] private MonacoEditorService Editor { get; init; } = default!;

        [Parameter]
        public string? Root { get; init; }

        [Parameter]
        public string[]? SrcFiles { get; init; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await Editor.InitializeAsync("container", "test", "csharp", "vs-dark", true, true);

                await Task.Delay(1100);
                IsInitialized = true;
                StateHasChanged();
            }
        }
    }
}
