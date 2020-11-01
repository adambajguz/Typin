namespace TypinExamples.Shared.Components
{
    using System;
    using Microsoft.AspNetCore.Components;

    public partial class Tab : ComponentBase
    {
        [CascadingParameter]
        private TabContainer? Parent { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public string Text { get; set; } = string.Empty;

        protected override void OnInitialized()
        {
            if (Parent is null)
                throw new ArgumentNullException(nameof(Parent), $"{nameof(Tab)} must exist within a {nameof(TabContainer)}.");

            base.OnInitialized();

            Parent.AddTab(this);
        }
    }
}
