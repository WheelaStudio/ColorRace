public static class StringExtensions
{
    public static string GetStringWithoutNewLines(this string self)
    {
        return self.Replace("\n", "");
    }
}
