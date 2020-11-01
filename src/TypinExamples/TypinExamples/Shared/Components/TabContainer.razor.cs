namespace TypinExamples.Shared.Components
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Components;

    public partial class TabContainer : ComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        public Tab? Active { get; set; }
        protected List<Tab> Tabs = new List<Tab>();

        public void AddTab(Tab tab)
        {
            Tabs.Add(tab);

            if (Tabs.Count == 1)
            {
                Active = tab;
            }

            StateHasChanged();
        }
    }
}
