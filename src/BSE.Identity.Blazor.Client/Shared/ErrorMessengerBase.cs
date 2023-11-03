using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace BSE.Identity.Blazor.Client.Shared
{
    public class ErrorMessengerBase : ComponentBase
    {
        public bool HasErrors { get; set; }
        protected ObservableCollection<string> Errors { get; set; } = new ObservableCollection<string>();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Errors.CollectionChanged += OnErrorCollectionChanged;
        }

        private void OnErrorCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<string> errors)
            {
                HasErrors = errors.Count > 0;
            }
        }
    }
}
