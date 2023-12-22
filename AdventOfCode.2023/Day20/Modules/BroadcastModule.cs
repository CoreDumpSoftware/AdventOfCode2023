namespace AdventOfCode.Y2023.Day20.Modules;

public class BroadcastModule(List<string> logs) : Module(logs)
{
    public override void Update(Pulse input)
    {
    }

    public override IEnumerable<IModule> Emit()
    {
        foreach (var output in Outputs)
        {
            yield return output;
        }
    }
}
