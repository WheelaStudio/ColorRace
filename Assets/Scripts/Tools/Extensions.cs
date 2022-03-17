using System;
using System.Collections.Generic;
public static class Extensions // расширения существующих типов
{
    public static void Shuffle<T>(this IList<T> list) // рандомное перемешивание list'а
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
    public static string GetStringWithoutNewLines(this string self) // удаление "новых линий" в строке
    {
        return self.Replace("\n", "");
    }
}
