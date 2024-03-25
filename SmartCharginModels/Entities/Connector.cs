namespace SmartCharginModels.Entities
{
    public class Connector : BaseEntity
    {
        public int ChargeStationId { get; set; }
        public int MaxCurrentInAmps { get; set; }
        public ChargeStation? ChargeStation { get; set; }
    }
}