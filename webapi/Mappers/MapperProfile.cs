using AutoMapper;
using Lib.Dtos;
using Lib.Models;

namespace webapi.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<GameToBeSavedDto, Game>();
        CreateMap<Credentials, Player>();
    }
}