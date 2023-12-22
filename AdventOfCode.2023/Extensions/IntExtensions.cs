using System.Numerics;
using System.Xml.Linq;

namespace AdventOfCode.Y2023.Extensions;

public static class IntExtensions
{
    /// <summary>
    /// Returns the count of bits in a given 32-bit integer.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public static int CountBits(this int i) =>
        BitOperations.PopCount((uint)i);

    /// <summary>
    /// Finds the indices of bits between two 32-bit integers.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static IEnumerable<int> GetBitDifferences(this int left, int right)
    {
        var xor = left ^ right;

        var index = 0;
        var bitFlag = 1;
        while (bitFlag <= xor)
        {
            if ((bitFlag & xor) > 0)
                yield return index;

            bitFlag <<= 1;
            index++;
        }
    }
}
