namespace AdventOfCode.Y2023.Day20.Modules;

public abstract class Module(List<string> logs) : IModule
{
    protected readonly List<string> _logs = logs;

    public string Name { get; init; }
    public virtual bool State { get; protected set; } = false;
    public List<IModule> Outputs { get; set; }

    public abstract void Update(Pulse input);

    public abstract IEnumerable<IModule> Emit();

    protected string GetStateString(bool state) =>
        state ? "-high->" : "-low->";

    protected string EnlistOutputNames() =>
        string.Join(", ", Outputs.Select(o => o.Name));

    public override string ToString() =>
        $"{Name} ({State})";
}