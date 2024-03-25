using AutoMapper;
using SmartCharginModels.Entities;
using SmartCharginModels.Models;

namespace SmartCharging.Configurations
{

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ChargeStationViewModel, ChargeStation>().ReverseMap();
            CreateMap<ConnectorViewModel, Connector>().ReverseMap();
            CreateMap<GroupViewModel, Group>().ReverseMap();
        }
    }

}
