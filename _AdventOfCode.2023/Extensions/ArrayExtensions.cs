namespace AdventOfCode2023.Extensions;

public static class ArrayExtensions
{
    public static T[] SubArray<T>(this T[] data, int index, int length)
    {
        var result = new T[length];
        Array.Copy(data, index, result, 0, length);

        return result;
    }

    public static void Print(this char[][] data)
    {
        for (var y = 0; y < data.Length; y++)
        {


            for (var x = 0; x < data[0].Length - 1; x++)
            {

            }
        }

        foreach (var line in data.Select(array => new string(array.Select(c => (c >= 32 && c <= 126) ? c : ' ').ToArray())))
        {
            Console.WriteLine(line);
        }
    }
}