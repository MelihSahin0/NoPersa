using SharedLibrary.Validations;
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

        [ForeignKey("ArticleId")]
        public long ArticleId { get; set; }

        [Required]
        public required Article Article { get; set; }

        [Required]
        [IntType(min: 1, max: 10)]
        public required int DefaultNumberOfBoxes { get; set; }

        public List<MonthlyOverview> MonthlyOverviews { get; set; } = [];

        [IntType(min: 0)]
        public int Position { get; set; }

        [Required]
        public bool TemporaryDelivery { get; set; }

        [Required]
        public bool TemporaryNoDelivery { get; set; }

        [ForeignKey("WorkdaysId")]
        public long WorkdaysId { get; set; }

        [Required]
        public required Weekday Workdays { get; set; }

        [ForeignKey("HolidaysId")]
        public long HolidaysId { get; set; }

        [Required]
        public required Weekday Holidays { get; set; }

        [ForeignKey("RouteId")]
        public long RouteId { get; set; }

        public Route? Route { get; set; }

        public List<CustomersLightDiet> CustomersLightDiets { get; set; } = [];

        public List<LightDiet> LightDiets { get; set; } = [];

        public List<CustomersMenuPlan> CustomerMenuPlans { get; set; } = [];

        public List<CustomersFoodWish> CustomersFoodWish { get; set; } = [];

        public List<FoodWish> FoodWishes { get; set; } = [];

        public BoxStatus? BoxStatus { get; set; }
    }
}
