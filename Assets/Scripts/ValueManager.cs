using UnityEngine;
using System.Collections.Generic;
public class ValueManager : MonoBehaviour
{
    private int cubeCount = 2;
    private float cubeSpeed = 0.15f;
    private Color targetColor;
    private List<Color> anotherColors;
    private int stepHSV = 120;
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
        var maxHSV = stepHSV;
        anotherColors = new List<Color>();
        for (int i = 0; i <= cubeCount; i++)
        {
            var pastStep = maxHSV - stepHSV;
            float randomHSVH() => Random.Range(pastStep, maxHSV) / 360f;
            anotherColors.Add(Color.HSVToRGB(randomHSVH(), Random.Range(0.6f, 1f), Random.Range(0.6f,1f)));
            maxHSV += stepHSV;
        }
        anotherColors.Shuffle();
        var currentIndex = Random.Range(0, anotherColors.Count);
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
            stepHSV = 360/(cubeCount+1);
            CubeCountChanged?.Invoke();
        }
        return cubeCount;     
    }
}
