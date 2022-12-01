using ASI.Contracts.SlurmFortress;
using AutoMapper;
using SlurmFortress.Core.Models;

namespace SlurmFortress.Core.MappingProfiles;

public sealed class SlurmProfile : Profile
{
    public SlurmProfile()
    {
        CreateMap<Slurm, SlurmView>()
            .ReverseMap()
            ;
    }
}