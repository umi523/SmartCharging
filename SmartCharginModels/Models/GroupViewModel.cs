namespace SmartCharginModels.Models
{
    public class GroupViewModel
    {
        public string? Name { get; set; }
        public int CapacityInAmps { get; set; }
        public List<ChargeStationViewModel>? ChargeStations { get; set; }
    }

    public class ChargeStationViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int GroupId { get; set; }
        public List<ConnectorViewModel>? Connectors { get; set; }
    }

    public class ConnectorViewModel
    {
        public int Id { get; set; }
        public int ChargeStationId { get; set; }
        public int MaxCurrentInAmps { get; set; }
    }
}
