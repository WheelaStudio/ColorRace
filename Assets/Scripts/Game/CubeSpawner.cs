using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class CubeSpawner : MonoBehaviour // спавнер кубиков
{
    [SerializeField] private GameObject cube; // префаб кубика
    private int targetColorIndex = -1; // индекс целевого цвета
    private int spawnCount = 0; // количество спавнов
    private readonly List<int> targetsColorIndexes = new List<int> { 0, 1 }; // возможные индексы целевого цвета
    public const int MaxCubeCount = 4; // максимальное количество кубиков за раз
    private ValueManager valueManager; // ссылка на valueManager
    private int cubesCount = 2; // текущее количество кубиков
    public static CubeSpawner Singleton { get; private set; } // глобальная ссылка на скрипт
    private readonly Dictionary<int, Vector3> scales = new Dictionary<int, Vector3> // возможные размеры кубика в зависимости от количества кубиков за раз
    {
        [2] = new Vector3(45f, 45f, 45f),
        [3] = new Vector3(30f, 30f, 30f),
        [4] = new Vector3(22f, 22f, 22f)
    };
    private readonly Dictionary<int, Vector3[]> spawnPositons = new Dictionary<int, Vector3[]> // возможные позиции спавна кубика в зависимости от количества кубиков за раз 
    {
        [2] = new Vector3[] { new Vector3(0.45f, 0.45f, -12f), new Vector3(-0.45f, 0.45f, -12f) },
        [3] = new Vector3[] { new Vector3(0.6f, 0.3f, -12f), new Vector3(0f, 0.3f, -12f), new Vector3(-0.6f, 0.3f, -12f) },
        [4] = new Vector3[] { new Vector3(0.7f, 0.22f, -12f), new Vector3(0.2375f, 0.22f, -12f), new Vector3(-0.2375f, 0.22f, -12f), new Vector3(-0.7f, 0.22f, -12f) }
    };
    private void Awake() // инициализация глобальной ссылки
    {
        Singleton = this;
    }
    private IEnumerator Start() // инициализация остальных значений, ожидание окончания туториала
    {
        valueManager = ValueManager.Singleton;
        var tutorialManager = TutorialManager.shared;
        valueManager.CubeCountChanged += delegate
        {
            targetsColorIndexes.Add(cubesCount);
        };
        if (!tutorialManager.TutorialCompleted)
            yield return new WaitUntil(() => tutorialManager.TutorialCompleted);
        UpdateColor(false);
        Spawn();
    }
    public void UpdateColor(bool init = true) // обновление цвета, количества кубиков
    {
        cubesCount = valueManager.GetCubeCount(spawnCount == 5 || spawnCount == 30);
        if (init)
            valueManager.InitColor();
    }
    public void Spawn() // спавн кубиков
    {
        var targets = new List<int>(targetsColorIndexes);
        if (targetColorIndex != -1)
        {
            if (Random.Range(0, 10) != 1)
                targets.Remove(targetColorIndex);
        }
        targetColorIndex = targets[Random.Range(0, targets.Count)];
        var anotherColors = valueManager.AnotherColors;
        var spawnPositions = spawnPositons[cubesCount];
        var scale = scales[cubesCount];
        var speed = valueManager.GetCubeSpeed(true);
        var cube = Instantiate(this.cube, spawnPositions[targetColorIndex], Quaternion.identity);
        cube.name += " targetColor";
        cube.transform.localScale = scale;
        var cubeController = cube.GetComponent<CubeController>();
        cubeController.speed = speed;
        cubeController.SetColor(valueManager.TargetColor, true);
        for (int i = 0; i < anotherColors.Length; i++)
        {
            if (i != targetColorIndex)
            {
                cube = Instantiate(this.cube, spawnPositions[i], Quaternion.identity);
                cube.transform.localScale = scale;
                cubeController = cube.GetComponent<CubeController>();
                cubeController.speed = speed;
                cubeController.SetColor(anotherColors[i], false);
            }
        }
        spawnCount++;
    }
}
