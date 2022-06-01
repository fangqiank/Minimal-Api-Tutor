using AutoMapper;
using MinimalApiTutor.Dtos;
using MinimalApiTutor.Models;

namespace MinimalApiTutor.Profiles;

public class CommandProfile: Profile
{
    public CommandProfile()
    {
        //source -> target
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<CommandUpdateDto, Command>();
    }
}
