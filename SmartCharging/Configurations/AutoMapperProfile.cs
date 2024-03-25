using AutoMapper;
using SmartCharginModels.Entities;
using SmartCharginModels.Models;

namespace SmartCharging.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GroupPostModel, Group>();
            CreateMap<GroupViewModel, Group>().ReverseMap();
            CreateMap<GroupViewModel, Group>().ReverseMap();
            CreateMap<ConnectorViewModel, Connector>().ReverseMap();
            CreateMap<ConnectorPostModel, Connector>();
            CreateMap<ChargeStationViewModel, ChargeStation>().ReverseMap();
            CreateMap<ChargeStationPostModel, ChargeStation>();
        }
    }

}
