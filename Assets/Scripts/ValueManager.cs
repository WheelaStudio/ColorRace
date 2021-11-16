using UnityEngine;
using System.Collections.Generic;
using Random = System.Random;
public class ValueManager : MonoBehaviour
{
    private int cubeCount = 2;
    private float cubeSpeed = 0.15f;
    private Color targetColor;
    private List<Color> anotherColors;
    private Random random;
    public delegate void cubeCountChanged();
    public event cubeCountChanged CubeCountChanged;
    public static ValueManager Singleton { get; private set; }
    private void Awake()
    {
        Singleton = this;
        InitColor();
    }
    public Color TargetColor
    {
        get
        {
            return targetColor;
        }
    }
    public Color[] AnotherColors
    {
        get
        {
            return anotherColors.ToArray();
        }
    }
    public void InitColor()
    {
        anotherColors = new List<Color>();
        random = new Random();
        for (int i = 0; i < cubeCount + 1; i++)
        {
            anotherColors.Add(new Color32((byte)random.Next(235), (byte)random.Next(235) , (byte)random.Next(235), 255));
        }
        var currentIndex = random.Next(0, anotherColors.Count);
        targetColor = anotherColors[currentIndex];
        anotherColors.RemoveAt(currentIndex);
    }
    public float GetCubeSpeed(bool increase)
    {
        if(increase)
            cubeSpeed += 0.001f;
            return cubeSpeed;
    }
    public int GetCubeCount(bool increase)
    {
        if (cubeCount != CubeSpawner.MaxCubeCount && increase)
        {
            cubeCount++;
            CubeCountChanged?.Invoke();
        }
        return cubeCount;     
    }
}
