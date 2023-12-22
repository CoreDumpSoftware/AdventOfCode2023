namespace AdventOfCode.Y2023.Day20.Modules;

public class FlipFlopModule(List<string> logs) : Module(logs)
{
    private bool _shouldEmit = false;

    public override void Update(Pulse input)
    {
        // When the input is high, ignore
        if (input.State)
            return;

        State = !State;
        _shouldEmit = true;
    }

    public override IEnumerable<IModule> Emit()
    {
        if (_shouldEmit)
        {
            foreach (var output in Outputs)
            {
                yield return output;
            }

            _shouldEmit = false;
        }
    }
}
