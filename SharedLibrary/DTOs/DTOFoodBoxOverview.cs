namespace SharedLibrary.DTOs
{
    public class DTOFoodBoxOverview
    {
        public string? RouteName { get; set; }

        public List<DTOBoxContentNumberOf>? BoxContents { get; set; }

        public List<DTOLightDietsNumberOf>? LightDiets { get; set; }
    }
}
