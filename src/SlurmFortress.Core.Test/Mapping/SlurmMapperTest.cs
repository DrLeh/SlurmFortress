using ASI.Contracts.SlurmFortress;
using SlurmFortress.Core.Models;

namespace SlurmFortress.Core.Test.Mapping;

public class SlurmMapperTest : MapperTest
{
    [Fact]
    public void ToViewModel_Test()
    {
        var source = new Slurm
        {
            Id = Guid.NewGuid(),
            CreateDate = new DateTime(2000, 1, 1)
        };

        var result = Mapper.Map<SlurmView>(source);

        result.Id.Should().Be(source.Id);
        result.CreateDate.Should().Be(new DateTime(2000, 1, 1));
    }

    [Fact]
    public void ToViewModel_NoAddress_Test()
    {
        var source = new Slurm
        {
            Id = Guid.NewGuid()
        };

        var result = Mapper.Map<SlurmView>(source);

        result.Id.Should().Be(source.Id);
    }
}
