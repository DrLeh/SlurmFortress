using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SlurmFortress.Core.MappingProfiles;

namespace SlurmFortress.Core.Test.Mapping;

public abstract class MapperTest
{
    private IMapper? _mapper;
    protected IMapper Mapper
    {
        get
        {
            if (_mapper != null)
                return _mapper;
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(SlurmProfile));
            return _mapper = services.BuildServiceProvider().GetRequiredService<IMapper>();
        }
    }
}
