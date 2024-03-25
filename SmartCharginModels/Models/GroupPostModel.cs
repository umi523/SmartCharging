namespace SmartCharginModels.Models
{
    public class GroupPostModel
    {
        public string? Name { get; set; }
        public int CapacityInAmps { get; set; }
        public List<ChargeStationPostModel>? ChargeStations { get; set; }
    }

    public class ChargeStationPostModel
    {
        public string? Name { get; set; }
        public List<ConnectorPostModel>? Connectors { get; set; }
    }

    public class ConnectorPostModel
    {
        public int MaxCurrentInAmps { get; set; }
    }
}
