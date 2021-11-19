using System;
using System.Collections.Generic;
public static class Extensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        var random = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
    public static string GetStringWithoutNewLines(this string self)
    {
        return self.Replace("\n", "");
    }
}
