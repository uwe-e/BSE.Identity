using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Fast.Components.FluentUI;

namespace BSE.Identity.Blazor.Client.Pages.Shared
{
    public class EditFormDialogBase : ComponentBase
    {
        [CascadingParameter]
        public FluentDialog? Dialog { get; set; }

        public ErrorMessenger? ErrorMessenger;

        protected virtual async Task HandleValidSubmit(EditContext editContext)
        {
            await Task.CompletedTask;
        }
        protected virtual async Task OnCancel()
        {
            if (Dialog is not null)
            {
                await Dialog.CancelAsync();
            }
        }
    }
}
