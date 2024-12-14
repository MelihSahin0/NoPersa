namespace SharedLibrary.DTOs.Gastro
{
    public class DTOFoodWish
    {
        public long Id { get; set; }

        public int Position { get; set; }

        public string? Name { get; set; }

        public bool? IsIngredient { get; set; }
    }
}
