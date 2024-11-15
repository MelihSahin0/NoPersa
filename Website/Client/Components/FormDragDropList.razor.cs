using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;
using Website.Client.Models;
using Website.Client.Styles;

namespace Website.Client.Components
{
    public partial class FormDragDropList
    {
        [CascadingParameter]
        private EditContext? CascadedEditContext { get; set; }

        [Parameter]
        public string MaxHeight { get; set; } = string.Empty;

        [Parameter]
        public string TextOrientation { get; set; } = "text-left";

        [Parameter]
        public string Class { get; set; } = string.Empty;

        [Parameter]
        public string? Placeholder { get; set; }

        [Parameter]
        public Func<string?, bool>? ValidationFunction { get; set; }

        [Parameter]
        public bool Draggable { get; set; } = true;  

        [Parameter]
        public bool DisplayFilter { get; set; } = false;

        [Parameter]
        public string StartFilter { get; set; } = string.Empty;

        [Parameter]
        public required List<RouteSummary> RouteOverviews { get; set; }

        [Parameter]
        public EventCallback<List<RouteSummary>> RouteOverviewsChanged { get; set; }

        private RouteSummary? draggedItem;
        private void HandleDrop(RouteSummary landingModel)
        {
            if (draggedItem is null)
            {
                return;
            }

            int originalOrderLanding = landingModel.Position;

            RouteOverviews.Where(x => x.Position >= landingModel.Position).ToList().ForEach(x => x.Position++);

            draggedItem.Position = originalOrderLanding;

            int i = 0;
            foreach (var route in RouteOverviews.OrderBy(x => x.Position).ToList())
            {
                route.Position = i++;

                route.IsDragOver = false;
            }
        }

        private bool IsPopupVisible = false;
        private int? toDeletePosition;
        private void DeleteRoute(int position)
        {
            toDeletePosition = position;

            if (RouteOverviews[position].NumberOfCustomers > 0)
            {
                IsPopupVisible = true;
            }
            else
            {
                DeleteRouteConfirmed();
            }
        }

        private void AddRoute()
        {
            RouteOverviews.Add(new RouteSummary() { Id = 0, Position = RouteOverviews.Count, Name = "", NumberOfCustomers = 0 });
        }

        public string ValidStateCss(Expression<Func<string>>? For)
        {
            if (For == null) 
            {
                InputStyles.GetBorderDefaultStyle(false);
            }

            var fieldIdentifier = FieldIdentifier.Create(For);
            var isInvalid = CascadedEditContext!.GetValidationMessages(fieldIdentifier).Any();

            return InputStyles.GetBorderDefaultStyle(isInvalid);
        }

        private void HandlePopupClose(bool result)
        {
            IsPopupVisible = false;
            
            if (result)
            {
                DeleteRouteConfirmed();  
            }
        }

        private void DeleteRouteConfirmed()
        {
            if (toDeletePosition != null)
            {
                RouteOverviews.Remove(RouteOverviews.FirstOrDefault(r => r.Position == toDeletePosition)!);

                int i = 0;
                foreach (var route in RouteOverviews.OrderBy(x => x.Position).ToList())
                {
                    route.Position = i++;

                    route.IsDragOver = false;
                }

                toDeletePosition = null;
            }
        }
    }
}
