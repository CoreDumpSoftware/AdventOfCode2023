using System.Diagnostics;

namespace AdventOfCode.Y2023.Day20.Modules;

[DebuggerStepThrough]
public class Pulse(string name, bool state)
{
    public string Name { get; set; } = name;
    public bool State { get; set; } = state;

    public override string ToString()
    {
        return Name + " " + (State
            ? "-high->"
            : "-low->");
    }
}
