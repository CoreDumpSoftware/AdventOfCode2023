namespace AdventOfCode.Y2023.Day20.Modules;

public interface IModule
{
    public string Name { get; init; }
    public bool State { get; }
    List<IModule> Outputs { get; set; }

    void Update(Pulse input);
    IEnumerable<IModule> Emit();
}
