namespace NoPersaService.DTOs.Gastro.Answer
{
    public class DTORoutesFoodSummary
    {
        public string? RouteName { get; set; }

        public List<DTOBoxContentSummary>? BoxContentSummary { get; set; }

        public List<DTOLightDietSummary>? LightDietSummary { get; set; }
    }
}
