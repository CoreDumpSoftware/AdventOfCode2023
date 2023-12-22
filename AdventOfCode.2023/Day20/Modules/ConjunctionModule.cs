namespace AdventOfCode.Y2023.Day20.Modules;

public class ConjunctionModule(List<string> logs) : Module(logs)
{
    private Dictionary<string, bool> _statesByInput;

    public override bool State
    {
        get => GetOutputState();
        protected set { }
    }

    public override void Update(Pulse input)
    {
        _statesByInput[input.Name] = input.State;
    }

    public override IEnumerable<IModule> Emit()
    {
        foreach (var output in Outputs)
        {
            yield return output;
        }
    }

    public void InitStateMemory(IEnumerable<string> moduleNames)
    {
        _statesByInput = moduleNames.Select(n => new KeyValuePair<string, bool>(n, false)).ToDictionary();
    }

    //public bool GetOutputState() =>
    //    _statesByInput.Any(x => x.Value == false);

    public bool GetOutputState()
    {
        // if all inputs are high, emit low
        // otherwise emit high

        return !_statesByInput.All(x => x.Value == true);
    }
}
