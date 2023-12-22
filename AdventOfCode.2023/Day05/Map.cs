using System.Data;

namespace AdventOfCode.Y2023.Day05;

public class Map
{
    public string SourceCategory { get; init; }
    public string DestinationCategory { get; init; }
    public List<MapRow> Mappings { get; init; }

    public Map(MapDefinition mapDefinition)
    {
        SourceCategory = mapDefinition.SourceCategory;
        DestinationCategory = mapDefinition.DestinationCategory;
        Mappings = mapDefinition.Mappings.Select(MapRow.Parse).OrderBy(m => m.Source.Start).ToList();
    }

    public long this[long source]
    {
        get
        {
            var mapRow = GetMapRowBySource(source);

            return mapRow != null
                ? mapRow.GetDestinationBySource(source)
                : source;
        }
    }

    private MapRow? GetMapRowBySource(long source) => Mappings.FirstOrDefault(m => m.Source.Contains(source));
}
