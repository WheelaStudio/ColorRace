using UnityEngine;
[CreateAssetMenu(fileName ="New SkinData", menuName = "Skin Data", order = 51)]
public class SkinData : ScriptableObject
{
    [SerializeField] private string skinName;
    [SerializeField] private int price;
    [SerializeField] private Texture skinTexture;
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
