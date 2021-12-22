using UnityEngine;
[CreateAssetMenu(fileName ="New SkinData", menuName = "Skin Data", order = 51)]
public class SkinData : ScriptableObject // класс с непосредственным описанием скина
{
    [SerializeField] private string skinName; // имя
    [SerializeField] private int price; // цена 
    [SerializeField] private Texture skinTexture; // текстура

    // свойства для получения значений
    public string Name
    {
        get
        {
            return skinName;
        }
    }
    public int Price
    {
        get
        {
            return price;
        }
    }
    public Texture SkinTexture 
    {
        get
        {
            return skinTexture;
        }
    }
}
