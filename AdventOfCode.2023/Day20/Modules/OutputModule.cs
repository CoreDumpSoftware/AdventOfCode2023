namespace AdventOfCode.Y2023.Day20.Modules;

public class OutputModule(List<string> logs) : Module(logs)
{
    public override void Update(Pulse input)
    {
        if (input.State == false)
            _logs.Add($"{Name} has received a low pulse!");
    }

    public override IEnumerable<IModule> Emit()
    {
        yield break;
    }
}