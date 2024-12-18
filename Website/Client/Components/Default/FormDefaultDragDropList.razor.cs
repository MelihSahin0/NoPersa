﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Website.Client.Styles;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Components.Default
{
    public partial class FormDefaultDragDropList<T>
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
        public bool HasDefault { get; set; } = false;

        [Parameter]
        public required List<DragDropInput<T>> DragDropInputs { get; set; }

        [Parameter]
        public EventCallback<List<DragDropInput<T>>> DragDropInputsChanged { get; set; }

        private DragDropInput<T>? draggedItem;
        private void HandleDrop(DragDropInput<T> landingModel)
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

            if (DragDropInputs.Count != 0 && !DragDropInputs.Any(d => d.IsDefault))
            {
                DragDropInputs.First().IsDefault = true;
            }
        }

        private void AddItem()
        {
            T id = default!;

            if (typeof(T) == typeof(double))
            {
                id = (T)(object)0.0;
            }
            else if (typeof(T) == typeof(int))
            {
                id = (T)(object)0;
            }
            else if (typeof(T) == typeof(long))
            {
                id = (T)(object)0L;
            }
            else if (typeof(T) == typeof(string))
            {
                id = (T)(object)"0";
            }

            DragDropInputs.Add(new DragDropInput<T>()
            {
                Id = id,
                Position = DragDropInputs.Count,
                Value = string.Empty,
                IsDefault = DragDropInputs.Count == 0
            });
        }

        private void SortByName()
        {
            int i = 0;
            foreach (var item in DragDropInputs.OrderBy(x => x.Value).ToList())
            {
                item.Position = i++;
            }
        }

        private void HandleIsDefault(DragDropInput<T> dragDropInput)
        {
            foreach (var item in DragDropInputs)
            {
                if (item == dragDropInput)
                {
                    item.IsDefault = true;
                }
                else
                {
                    item.IsDefault = false;
                }
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

    public class DragDropInput<T>
    {
        [Required]
        public required T Id { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Value { get; set; }

        public bool IsDefault { get; set; } = false;

        public bool IsDragOver { get; set; }
    }
}
