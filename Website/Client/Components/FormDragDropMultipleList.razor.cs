using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Website.Client.Models;

namespace Website.Client.Components
{
    public partial class FormDragDropMultipleList
    {
        [CascadingParameter]
        private EditContext? CascadedEditContext { get; set; }

        [Parameter]
        public string MaxHeight { get; set; } = string.Empty;

        [Parameter]
        public string MinHeight { get; set; } = string.Empty;

        [Parameter]
        public string TextOrientation { get; set; } = "text-left";

        [Parameter]
        public string Class { get; set; } = string.Empty;

        [Parameter]
        public bool Draggable { get; set; } = true;

        [Parameter]
        public bool DisplayFilter { get; set; } = false;

        [Parameter]
        public string[]? StartFilter { get; set; }

        [Parameter]
        public required List<SequenceDetails> Routes { get; set; } = [];

        [Parameter]
        public EventCallback<List<SequenceDetails>> RoutesChanged { get; set; }

        [Parameter]
        public required int[]? SelectedRouteId { get; set; }

        private CustomersSequence? draggedItem;
        private void HandleDrop(CustomersSequence landingModel, int targetId)
        {
            if (draggedItem is null)
            {
                return;
            }

            var sourceRoute = Routes.Find(r => r.CustomersRoute.Contains(draggedItem));
            var targetRoute = Routes.Find(r => r.CustomersRoute.Contains(landingModel));

            int originalLanding = 0;
            if (sourceRoute == null)
            {
                return;
            }
            if (targetRoute == null)
            {
                foreach (var routeId in SelectedRouteId ?? [])
                {
                    if (Routes.FirstOrDefault(r => r.Id == routeId)!.CustomersRoute.Count == 0)
                    {
                        targetRoute = Routes.FirstOrDefault(r => r.Id == routeId);
                        break;
                    }
                }
                
                if (targetRoute == null)
                {
                    return;
                }

                if (targetId == sourceRoute.Id && targetRoute.CustomersRoute.Count == 0)
                {
                    return;
                }
            }
            else
            {
                originalLanding = landingModel.Position;
            }

            if (sourceRoute.Id == targetRoute.Id)
            {
                sourceRoute.CustomersRoute.Where(x => x.Position >= landingModel.Position).ToList().ForEach(x => x.Position++);

                draggedItem.Position = originalLanding;
            }
            else
            {
                Console.WriteLine("test");
                sourceRoute.CustomersRoute.Remove(draggedItem);

                targetRoute.CustomersRoute.Where(x => x.Position >= landingModel.Position).ToList().ForEach(x => x.Position++);

                draggedItem.Position = originalLanding;
                targetRoute.CustomersRoute.Add(draggedItem);

                int i = 0;
                foreach (var customer in sourceRoute.CustomersRoute.OrderBy(c => c.Position).ToList())
                {
                    customer.Position = i++;
                    customer.IsDragOver = false;
                }
            }

            int j = 0;
            foreach (var customer in targetRoute.CustomersRoute.OrderBy(c => c.Position).ToList())
            {
                customer.Position = j++;
                customer.IsDragOver = false;
            }
        }
    }
}
