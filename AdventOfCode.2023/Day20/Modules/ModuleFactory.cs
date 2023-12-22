using AdventOfCode.Common.DataStructures;
using AdventOfCode.Common.Extensions;

namespace AdventOfCode.Y2023.Day20.Modules;

public class ModuleFactory
{
    public Button CreateNetwork(IEnumerable<string> input, Action<IEnumerable<string>> onLog)
    {
        var logLines = new List<string>();
        var modulesByName = new DefaultDictionary<string, (IModule Module, IEnumerable<string> Inputs)>();

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            try
            {
                var parts = line.Split(" ");
                var type = parts[0][0];
                var name = type == '%' || type == '&' ? parts[0][1..] : parts[0];
                var connectionNames = parts[2..].Select(p => p.Replace(",", "")).ToArray();

                IModule module = type switch
                {
                    '%' => new FlipFlopModule(logLines) { Name = name },
                    '&' => new ConjunctionModule(logLines) { Name = name },
                    _ => new BroadcastModule(logLines) { Name = name },
                };

                modulesByName[module.Name] = (module, connectionNames)!;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        var button = new Button(
            "button",
            modulesByName.Select(kvp => new KeyValuePair<string, IModule>(kvp.Key, kvp.Value.Module)).ToDictionary(),
            logLines,
            onLog);

        BuildModuleTree(modulesByName, "broadcaster");
        RegisterConjuctionInputs(modulesByName);

        return button;
    }

    private void BuildModuleTree(Dictionary<string, (IModule Module, IEnumerable<string> Outputs)> modulesByName, string toBuild)
    {
        if (!modulesByName.TryGetValue(toBuild, out var tuple))
        {
            tuple.Item1 = new OutputModule(null!) { Name = toBuild, Outputs = [] };
            tuple.Item2 = [];
            modulesByName.Add(toBuild, tuple);
        }

        (IModule module, IEnumerable<string> connections) = modulesByName[toBuild];

        if (module.Outputs.SafeAny())
            return;

        module.Outputs = connections.Select(c =>
        {
            if (!modulesByName.TryGetValue(c, out var tuple))
            {
                tuple.Module = new OutputModule(null!) { Name = c, Outputs = [] };
                tuple.Outputs = [];

                modulesByName.Add(tuple.Module.Name, tuple);
            }

            return tuple.Module;
        }).ToList();

        foreach (var output in module.Outputs)
        {
            BuildModuleTree(modulesByName, output.Name);
        }
    }

    private void RegisterConjuctionInputs(Dictionary<string, (IModule Module, IEnumerable<string> Outputs)> modulesByName)
    {
        var modules = modulesByName.Select(kvp => kvp.Value.Module)
            .ToList();

        var conjuctionModules = modules
            .Where(m => m is ConjunctionModule)
            .Cast<ConjunctionModule>();

        foreach (var module in conjuctionModules)
        {
            module.InitStateMemory(modules.Where(m => m.Outputs.Any(o => o.Name == module.Name)).Select(o => o.Name));
        }
    }
}
