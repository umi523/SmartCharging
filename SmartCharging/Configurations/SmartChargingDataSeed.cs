using Newtonsoft.Json;
using SmartCharginModels.Entities;
using SmartCharginRepository.DatabaseContext;

namespace SmartCharging.Configurations
{
    public static class SmartChargingDataSeed
    {
        public static void Initialize(SmartCharginDbContext context)
        {
            var seedDataJson = File.ReadAllText("seedData.json");
            var groups = JsonConvert.DeserializeObject<List<Group>>(seedDataJson);

            foreach (var group in groups ?? [])
            {
                context.Groups.Add(group);

                foreach (var chargeStation in group?.ChargeStations ?? [])
                {
                    context.ChargeStations.Add(chargeStation);

                    foreach (var connector in chargeStation?.Connectors ?? [])
                    {
                        context.Connectors.Add(connector);
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
