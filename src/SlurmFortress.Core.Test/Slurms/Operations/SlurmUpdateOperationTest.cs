using SlurmFortress.Core.Models;
using SlurmFortress.Core.Slurms;
using SlurmFortress.Core.Slurms.Operations;

namespace SlurmFortress.Core.Test.Slurms.Operations;

public class SlurmUpdateOperationTest
{
    [Fact]
    public void Slurm_Update_Test()
    {
        //AAA
        //Arrange - Act - Assert

        //arrange
        var id = Guid.NewGuid();
        var trans = new TestDataTransaction();
        var loader = new Mock<ISlurmLoader>();

        loader.Setup(x => x.Get(id)).Returns(new Slurm
        {
        });

        var ctx = new SlurmUpdateContext(id, e =>
        {
        }, loader.Object, trans);

        var op = new SlurmUpdateOperation(ctx);

        //Act
        op.Load();
        op.StageChanges();

        //Assert
        trans.UpdatedEntityOfType<Slurm>().Count().Should().Be(1);
    }

    [Fact]
    public void Slurm_Load_Test()
    {
        //AAA
        //Arrange - Act - Assert

        //arrange
        var id = Guid.NewGuid();
        var trans = new TestDataTransaction();
        var loader = new Mock<ISlurmLoader>();

        loader.Setup(x => x.Get(id)).Returns(new Slurm
        {
        }).Verifiable();

        var ctx = new SlurmUpdateContext(id, e =>
        {
        }, loader.Object, trans);

        var op = new SlurmUpdateOperation(ctx);

        //Act
        op.Load();

        loader.VerifyAll();
    }
}
