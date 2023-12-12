namespace webapi.Hubs.Static;

public static class RandomGenerator
{
    private static readonly Random Generator = new();

    public static int Draw(int minValue, int maxValue)
    {
        return Generator.Next(minValue, maxValue);
    }
}