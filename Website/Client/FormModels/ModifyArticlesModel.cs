using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using Website.Client.Enums;
using Website.Client.Models;

namespace Website.Client.FormModels
{
    public class ModifyArticlesModel
    {
        [ValidateComplexType]
        [Required]
        [MinChildren(1, ErrorMessage = "This requires one article")]
        public required List<ArticleSummary> Articles { get; set; }

        [Required]
        public required bool IsTaskSet { get; set; }

        [Required]
        public required int Year { get; set; }

        [Required]
        public required Months Month { get; set; }

        [Required]
        public required int Day { get; set; }
    }
}
