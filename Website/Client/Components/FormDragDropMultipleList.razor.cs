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
        public required List<SequenceDetail> SequenceDetails { get; set; } = [];

        [Parameter]
        public EventCallback<List<SequenceDetail>> SequenceDetailsChanged { get; set; }

        [Parameter]
        public required int[]? SelectedRouteId { get; set; }

        private CustomerSequence? draggedItem;
        private void HandleDrop(CustomerSequence landingModel, int targetId)
        {
            if (draggedItem is null)
            {
                return;
            }

            var sourceRoute = SequenceDetails.Find(r => r.CustomerSequence.Contains(draggedItem));
            var targetRoute = SequenceDetails.Find(r => r.CustomerSequence.Contains(landingModel));

            int originalLanding = 0;
            if (sourceRoute == null)
            {
                return;
            }
            if (targetRoute == null)
            {
                foreach (var routeId in SelectedRouteId ?? [])
                {
                    if (SequenceDetails.FirstOrDefault(r => r.Id == routeId)!.CustomerSequence.Count == 0)
                    {
                        targetRoute = SequenceDetails.FirstOrDefault(r => r.Id == routeId);
                        break;
                    }
                }
                
                if (targetRoute == null)
                {
                    return;
                }

                if (targetId == sourceRoute.Id && targetRoute.CustomerSequence.Count == 0)
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
                sourceRoute.CustomerSequence.Where(x => x.Position >= landingModel.Position).ToList().ForEach(x => x.Position++);

                draggedItem.Position = originalLanding;
            }
            else
            {
                sourceRoute.CustomerSequence.Remove(draggedItem);

                targetRoute.CustomerSequence.Where(x => x.Position >= landingModel.Position).ToList().ForEach(x => x.Position++);

                draggedItem.Position = originalLanding;
                targetRoute.CustomerSequence.Add(draggedItem);

                int i = 0;
                foreach (var customer in sourceRoute.CustomerSequence.OrderBy(c => c.Position).ToList())
                {
                    customer.Position = i++;
                    customer.IsDragOver = false;
                }
            }

            int j = 0;
            foreach (var customer in targetRoute.CustomerSequence.OrderBy(c => c.Position).ToList())
            {
                customer.Position = j++;
                customer.IsDragOver = false;
            }
        }
    }
}
