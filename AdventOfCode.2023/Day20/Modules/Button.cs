using AdventOfCode.Y2023.Extensions;

namespace AdventOfCode.Y2023.Day20.Modules;

public class Button(
    string name,
    Dictionary<string, IModule> modules,
    List<string> logLines,
    Action<IEnumerable<string>> onLog)
{
    private readonly Action<IEnumerable<string>> _onLog = onLog;
    private List<string> logLines { get; set; } = logLines;

    public string Name { get; set; } = name;
    public Dictionary<string, IModule> Modules { get; set; } = modules;

    public long Push(int pushCount = 1)
    {
        var broadcaster = Modules["broadcaster"];

        var queue = new Queue<IModule>();
        var totalLowPulses = 0;
        var totalHighPulses = 0;

        for (var i = 0; i < pushCount; i++)
        {
            var lowPulseCount = 1;
            var highPulseCount = 0;
            queue.Enqueue(broadcaster);

            while (queue.Count > 0)
            {
                var module = queue.Dequeue();
                var pulse = new Pulse(module.Name, module.State);

                foreach (var output in module.Emit())
                {
                    if (pulse.State)
                        highPulseCount++;
                    else
                        lowPulseCount++;

                    queue.Enqueue(output);
                    output.Update(pulse);
                }
            }

            totalHighPulses += highPulseCount;
            totalLowPulses += lowPulseCount;
        }

        var product = totalLowPulses * totalHighPulses;

        return product;
    }

    public long PushAndWaitForOutputTrigger()
    {
        var broadcaster = Modules["broadcaster"];
        var queue = new Queue<IModule>();
        long pushCount = 0;

        // Looked up the nodes connected to the final conjuction before the output
        // Find out when they first emit a low signal for each input, then calculate
        // the LCM.
        var conjunctions = new List<string> { "vf", "rn", "dh", "mk" };
        var firstLows = new List<long>();

        while (true)
        {
            pushCount++;
            queue.Enqueue(broadcaster);

            while (queue.Count > 0)
            {
                if (conjunctions.Count == 0)
                    break;

                var module = queue.Dequeue();
                var pulse = new Pulse(module.Name, module.State);

                if (conjunctions.Contains(module.Name) && pulse.State == true)
                {
                    conjunctions.Remove(module.Name);
                    firstLows.Add(pushCount);
                }

                foreach (var output in module.Emit())
                {
                    queue.Enqueue(output);
                    output.Update(pulse);
                }
            }

            if (conjunctions.Count == 0)
                break;
        }

        return firstLows.LeastCommonMultiple();
    }
}