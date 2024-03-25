namespace SmartCharginModels.Entities
{
    public class ChargeStation : BaseEntity
    {
        public int GroupId { get; set; }
        public Group? Group { get; set; }
        public string? Name { get; set; }
        public List<Connector>? Connectors { get; set; }
    }
}
