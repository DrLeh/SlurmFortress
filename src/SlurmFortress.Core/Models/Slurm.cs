namespace SlurmFortress.Core.Models;

public class Slurm : Entity
{

}

public class QueenSlurm : Slurm
{
    public int SpawnRate { get; set; } = 1;

    public void Produce()
    {
        Children.Add(new());
    }

    public List<Slurm> Children { get; set; } = new();
}
