using UnityEngine;
using System.Collections.Generic;
public class ValueManager : MonoBehaviour // данные игры
{
    private int cubeCount = 2; // количество кубиков в данный момент
    private float cubeSpeed = 0.15f; // скорость кубика
    private Color targetColor; // целевой цвет
    private List<Color> anotherColors; // остальные цвета
    private int stepHSV = 120; // "шаг" в тоне во время генерации цвета
    public delegate void cubeCountChanged(); // делегат, срабатывающий при смене количества кубиков
    public event cubeCountChanged CubeCountChanged; // событие, оповещающее о смене количества кубиков
    public static ValueManager Singleton { get; private set; } // глобальная ссылка на скрипт
    private void Awake() // инициализация значений
    {
        Singleton = this;
        InitColor();
    }
    public Color TargetColor // свойство, возвращающее целевой цвет
    {
        get
        {
            return targetColor;
        }
    }
    public Color[] AnotherColors // свойство, возвращающее остальные цвета 
    {
        get
        {
            return anotherColors.ToArray();
        }
    }
    public void InitColor() // иницализация цветов 
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
    public float GetCubeSpeed(bool increase) // получить скорость кубика(увеличить)
    {
        if(increase)
            cubeSpeed += 0.001f;
            return cubeSpeed;
    }
    public int GetCubeCount(bool increase) // получить количество кубиков(увеличить)
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
