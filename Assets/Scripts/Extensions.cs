using System;
public static class Extensions
{
    public static string GetStringWithoutNewLines(this string self)
    {
        return self.Replace("\n", "");
    }
    public static byte GetRandomBytesWithStep(this Random random, int min, int max, double step)
    {
        int n = (int)((max - min) / step);
        int r = random.Next(0, n);
        return (byte)(min + r * step);
    }
}
