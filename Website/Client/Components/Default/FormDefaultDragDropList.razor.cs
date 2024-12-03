using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Website.Client.Styles;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Components.Default
{
    public partial class FormDefaultDragDropList
    {
        [CascadingParameter]
        private EditContext? CascadedEditContext { get; set; }

        [Parameter]
        public string? RemoveButtonDisplay { get; set; }

        [Parameter]
        public string? AddButtonDisplay { get; set; }

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
        public required List<DragDropInput> DragDropInputs { get; set; }

        [Parameter]
        public EventCallback<List<DragDropInput>> DragDropInputsChanged { get; set; }

        private DragDropInput? draggedItem;
        private void HandleDrop(DragDropInput landingModel)
        {
            if (draggedItem is null)
            {
                return;
            }

            int originalOrderLanding = landingModel.Position;

            DragDropInputs.Where(x => x.Position >= landingModel.Position).ToList().ForEach(x => x.Position++);

            draggedItem.Position = originalOrderLanding;

            int i = 0;
            foreach (var item in DragDropInputs.OrderBy(x => x.Position).ToList())
            {
                item.Position = i++;
                item.IsDragOver = false;
            }
        }

        private void DeleteItem(int position)
        {
            DragDropInputs.Remove(DragDropInputs.FirstOrDefault(r => r.Position == position)!);

            int i = 0;
            foreach (var item in DragDropInputs.OrderBy(x => x.Position).ToList())
            {
                item.Position = i++;
            }
        }

        private void AddItem()
        {
            DragDropInputs.Add(new DragDropInput() { Id = 0, Position = DragDropInputs.Count, Value = ""});
        }

        private void SortByName()
        {
            int i = 0;
            foreach (var item in DragDropInputs.OrderBy(x => x.Value).ToList())
            {
                item.Position = i++;
            }
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
    }

    public class DragDropInput
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Value { get; set; }

        public bool IsDragOver { get; set; }
    }
}
