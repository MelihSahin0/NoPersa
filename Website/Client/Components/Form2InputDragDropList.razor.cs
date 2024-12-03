using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Website.Client.Models;
using Website.Client.Styles;
using Website.Client.Components.Default;

namespace Website.Client.Components
{
    public partial class Form2InputDragDropList
    {
        [CascadingParameter]
        private EditContext? CascadedEditContext { get; set; }

        [Parameter]
        public string Class { get; set; } = string.Empty;

        [Parameter]
        public string MaxHeight {  get; set; } = string.Empty;

        [Parameter]
        public bool Draggable { get; set; } = true;

        [Parameter]
        public string TitleLeft { get; set; } = string.Empty;

        [Parameter]
        public string TitleRight { get; set; } = string.Empty;

        [Parameter]
        public string? PlaceholderLeft { get; set; }

        [Parameter]
        public string? PlaceholderRight { get; set; }

        [Parameter]
        public Func<string?, bool>? ValidationLeftFunction { get; set; }

        [Parameter]
        public Func<string?, bool>? ValidationRightFunction { get; set; }

        [Parameter]
        public required List<ArticleSummary> ArticleSummary { get; set; }

        [Parameter]
        public EventCallback<List<ArticleSummary>> ArticleSummaryChanged { get; set; }

        private ArticleSummary? draggedItem;
        private void HandleDrop(ArticleSummary landingModel)
        {
            if (draggedItem is null)
            {
                return;
            }

            int originalOrderLanding = landingModel.Position;

            ArticleSummary.Where(x => x.Position >= landingModel.Position).ToList().ForEach(x => x.Position++);

            draggedItem.Position = originalOrderLanding;

            int i = 0;
            foreach (var article in ArticleSummary.OrderBy(x => x.Position).ToList())
            {
                article.Position = i++;

                article.IsDragOver = false;
            }
        }

        private bool IsPopupVisible = false;
        private int? toDeletePosition;
        private void DeleteArticle(int position)
        {
            toDeletePosition = position;

            if (ArticleSummary[position].NumberOfCustomers > 0)
            {
                IsPopupVisible = true;
            }
            else
            {
                DeleteArticleConfirmed();
            }
        }

        private void AddArticle()
        {
            ArticleSummary.Add(new ArticleSummary() { Id = 0, Position = ArticleSummary.Count, Name = "", Price = "", NewName = "", NewPrice = "", NumberOfCustomers = 0, IsDisabled = false });
        }

        private void SortByName()
        {
            int i = 0;
            foreach (var item in ArticleSummary.OrderBy(x => x.Name).ToList())
            {
                item.Position = i++;
            }
        }

        private string ValidStateCss(Expression<Func<string>>? For)
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
                DeleteArticleConfirmed();
            }
        }

        private void DeleteArticleConfirmed()
        {
            if (toDeletePosition != null)
            {
                ArticleSummary.Remove(ArticleSummary.FirstOrDefault((Func<ArticleSummary, bool>)(r => r.Position == toDeletePosition))!);

                int i = 0;
                foreach (var article in ArticleSummary.OrderBy(x => x.Position).ToList())
                {
                    article.Position = i++;

                    article.IsDragOver = false;
                }

                toDeletePosition = null;
            }
        }

        private void NameChanged(string value, ArticleSummary article)
        {
            if (!article.IsDisabled)
            {
                article.NewName = value;
            }
        }

        private void PriceChanged(string value, ArticleSummary article)
        {
            if (!article.IsDisabled)
            {
                article.NewPrice = value;
            }
        }
    }
}
