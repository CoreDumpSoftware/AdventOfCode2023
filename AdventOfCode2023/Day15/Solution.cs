using AdventOfCode2023.Extensions;

namespace AdventOfCode2023.Day15;

public class Solution : SolutionBase
{
    protected override string SolutionInput { get; init; } = "15.txt";
    protected override string SampleInputOne { get; set; } = "15_sample.txt";

    public class Box
    {
        private LensNode _root = null!;
        private LensNode _last = null!;

        public Lens? GetByLabel(string label)
        {
            return LookupNode(label)?.Lens;
        }

        public void AddOrUpdate(string label, int focalLength)
        {
            var node = LookupNode(label);
            if (node == null)
            {
                var lens = new Lens(label, focalLength);

                if (_root == null)
                {
                    _root = new LensNode { Lens = lens };
                    _last = _root;
                }
                else
                {
                    var next = new LensNode { Lens = lens, Previous = _last };
                    _last.Next = next;
                    _last = next;
                }
            }
            else
            {
                node.Lens.FocalLength = focalLength;
            }
        }

        public void Remove(string label)
        {
            var node = LookupNode(label);
            if (node == null)
                return;

            var prev = node.Previous;
            var next = node.Next;

            if (prev == null) // removing root...
            {
                _root = next;

                return;
            }

            if (node == _last)
                _last = node.Previous!;

            if (prev != null)
            {
                prev.Next = next;
            }
            if (next != null)
            {
                next.Previous = prev;
            }


        }

        public IEnumerable<Lens> GetLenses()
        {
            var node = _root;
            while (node != null)
            {
                yield return node.Lens;
                node = node.Next;
            }
        }

        private LensNode LookupNode(string label)
        {
            var node = _root;
            while (node != null && node.Lens.Label != label)
                node = node.Next;

            return node;
        }
    }

    public class LensNode
    {
        public Lens Lens { get; set; }
        public LensNode? Previous { get; set; }
        public LensNode? Next { get; set; }
    }

    public class Lens(string label, int focalLength)
    {
        public string Label { get; init; } = label;
        public int FocalLength { get; set; } = focalLength;

        public override string ToString() => $"[{Label} {FocalLength}]";
    }

    public override long PartOne()
    {
        var knownHashes = new Dictionary<string, int>();
        var line = GetFileContents(SolutionInput).First();
        using var stringReader = new StringReader(line);
        var token = stringReader.ReadUntil(',');
        var sum = 0;

        while (!string.IsNullOrEmpty(token.Value))
        {
            var result = CalculateHash(token.Value);
            sum += result;

            token = stringReader.ReadUntil(',');
        }

        return sum;
    }

    public override long PartTwo()
    {
        var line = GetFileContents(SolutionInput).First();
        using var stringReader = new StringReader(line);
        var token = stringReader.ReadUntil(',');

        var boxes = new Box[256];

        while (!string.IsNullOrEmpty(token.Value))
        {
            var last = token.Value[^1];
            var isRemove = last == '-';
            var label = token.Value[..^(isRemove ? 1 : 2)];
            var labelHash = CalculateHash(label);
            var box = boxes[labelHash];

            // remove
            if (last == '-' && box != null)
            {
                box.Remove(label);
            }
            // add or update
            else
            {
                var focalLength = last - '0';

                if (box == null)
                {
                    box = new Box();
                    boxes[labelHash] = box;
                }

                box.AddOrUpdate(label, focalLength);
            }

            token = stringReader.ReadUntil(',');
        }

        var sum = 0;
        for (var i = 0; i < 256; i++)
        {
            var box = boxes[i];
            if (box == null)
                continue;

            var result = 0;
            foreach ((var lens, var index) in box.GetLenses().Select((l, i) => (l, i)))
            {
                sum += (i + 1) * (index + 1) * lens.FocalLength;
            }
        }

        return sum;
    }

    public int CalculateHash(string input)
    {
        var result = 0;

        foreach (var c in input)
        {
            result += c;
            result *= 17;
            result %= 256;
        }

        return result;
    }

}
