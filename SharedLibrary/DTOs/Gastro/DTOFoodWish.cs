namespace SharedLibrary.DTOs.Gastro
{
    public class DTOFoodWish
    {
        public int Id { get; set; }

        public int Position { get; set; }

        public string? Name { get; set; }

        public bool? IsIngredient { get; set; }
    }
}
