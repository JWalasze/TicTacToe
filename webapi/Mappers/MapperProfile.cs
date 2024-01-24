using AutoMapper;
using Lib.Dtos;
using Lib.Models;

namespace webapi.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<GameInitialStateDto, Game>().ForMember(initialGame => initialGame.Id, 
            act => act.MapFrom(src => src.GameId));
        CreateMap<Credentials, Player>();
    }
}