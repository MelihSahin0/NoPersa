using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(64)]
        public string? SerialNumber { get; set; }

        [MaxLength(64)]
        public string? Title { get; set; }
        
        [Required]
        [MaxLength(64)]
        public required string Name {  get; set; }

        public DeliveryLocation? DeliveryLocation { get; set; }

        [MaxLength(64)]
        public string? ContactInformation { get; set; }

        [Required]
        [ForeignKey("ArticleId")]
        public required long ArticleId { get; set; }

        public Article? Article { get; set; }

        [Required]
        public required int DefaultNumberOfBoxes { get; set; }

        public List<MonthlyOverview> MonthlyOverviews { get; set; } = [];

        [Required]
        public required int Position { get; set; }

        [Required]
        public required bool TemporaryDelivery { get; set; }

        [Required]
        public required bool TemporaryNoDelivery { get; set; }

        [Required]
        [ForeignKey("WorkdaysId")]
        public required long WorkdaysId { get; set; }

        public Weekday? Workdays { get; set; }

        [Required]
        [ForeignKey("HolidaysId")]
        public long HolidaysId { get; set; }

        public Weekday? Holidays { get; set; }

        [Required]
        [ForeignKey("RouteId")]
        public required long RouteId { get; set; }

        public Route? Route { get; set; }

        public List<CustomersLightDiet> CustomersLightDiets { get; set; } = [];

        public List<LightDiet> LightDiets { get; set; } = [];

        public List<CustomersMenuPlan> CustomerMenuPlans { get; set; } = [];

        public List<CustomersFoodWish> CustomersFoodWish { get; set; } = [];

        public List<FoodWish> FoodWishes { get; set; } = [];

        public BoxStatus? BoxStatus { get; set; }
    }
}
