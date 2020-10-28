namespace TypinExamples.Pages
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using TypinExamples.Services;

    public partial class Index
    {
        [Inject] private XTermService Terminal { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                Terminal.Initialize("terminal");
                Terminal.Write("terminal", "test\n");
            }
        }
    }
}
