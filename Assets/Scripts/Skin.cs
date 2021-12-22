public struct Skin // описание скина
{
    public const int СheapestPrice = 100; // самая низкая цена
    public readonly SkinData skinData; // детальная информация о скине
    public bool isPurchased; // куплен ли скин
    public Skin(SkinData skinData, bool isPurchased) // конструктор для инициализации значений
    {
        this.skinData = skinData;
        this.isPurchased = isPurchased;
    }
}
