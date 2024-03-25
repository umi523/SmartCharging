namespace SmartCharginModels.Entities
{
    public class Group : BaseEntity
    {
        public string? Name { get; set; }
        public int CapacityInAmps { get; set; }
        public List<ChargeStation>? ChargeStations { get; set; }
    }
}