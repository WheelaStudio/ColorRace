using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class BallController : MonoBehaviour // контроль шарика
{
    private readonly Color white = new Color(1f, 1f, 1f, 1f); // белый цвет
    private readonly Color effectColor = new Color(1f, 1f, 1f, 0.25f); // цвет во время эффекта
    private bool isInEffect = false; // находится ли шарик в состоянии эффекта
    public static BallController Singleton { get; private set; } // глобальная ссылка на скрипт
    private Collider mainCollider; // коллайдер шарика
    private int timeForEffect; // время, в течение которого эффект недоступен
    [SerializeField] private GameObject effectTimeViewGameObject, LeftMovePanel, RightMovePanel; // представление отсчёта времени, панель для движения влево, вправо
    [SerializeField] private TextMeshProUGUI effectTimeView; // текст отсчёта времени до "доступности" эффекта
    [SerializeField] private Texture[] skins; // скины
    private Material LeftWall, RightWall; // материалы левой, правой стены
    [SerializeField] private AudioSource DestroyCubeSound; // звук уничтожения кубика
    [SerializeField] private MeshRenderer LeftWallRdr, RightWallRdr; // рендеры левой, правой стены
    [SerializeField] private ParticleSystem SnowSystem; // система частиц
    private MeshRenderer mainMeshRenderer; // рендер шарика
    private Rigidbody body; // ригидбоди шарика
    private Transform currentTransform; // трансформ шарика
    private ValueManager valueManager; // ссылка на valueManage
    private CubeSpawner cubeSpawner; // ссылка на спавнер кубиков
    private int posIndex = 0; // индекс на позицию шарика
    private bool isMoving = false; // двигается ли в сторону шарика
    private Game game; // ссылка на Game
    private bool snowSystemEnabled = false; // работает ли система партиклей
    private Dictionary<int, float[]> raceXPos = new Dictionary<int, float[]>() // словарь, в котором описаны возможные позиции по x в зависимости от кол-ва кубиков
    {
        [2] = new float[] { 0.5f, -0.5f },
        [3] = new float[] { 0.6f, 0f, -0.6f },
        [4] = new float[] { 0.67f, 0.25f, -0.25f, -0.67f }
    };
    public bool IsInEffect // свойство, возвращающее isInEffect 
    {
        get
        {
            return isInEffect;
        }
    }
    public bool IsTimeTextActive // свойство, возврающее значение, указывающее, включен или выключен текст отсчёта
    {
        get
        {
            return effectTimeViewGameObject.activeSelf;
        }
    }
    private bool CanMove // свойство, показывающее, может ли шарик двигаться в сторону
    {
        get
        {
            string leftTag = "", rightTag = "";
            if (Physics.Raycast(body.position, Vector3.left, out RaycastHit leftHit, 1f))
            {
                leftTag = leftHit.collider.tag;
            }
            if (Physics.Raycast(body.position, Vector3.right, out RaycastHit rightHit, 1f))
            {
                rightTag = rightHit.collider.tag;
            }
            return leftTag != "Cube" && rightTag != "Cube";
        }
    }
    private void Awake() // инициализация ссылки на скрипт
    {
        Singleton = this;
    }
    private IEnumerator Start() // инициализация остальных значений, ожидание окончания туториала
    {
        var skinIndex = Preferences.EquipedSkin;
        mainMeshRenderer = GetComponent<MeshRenderer>();
        mainCollider = GetComponent<Collider>();
        if (skinIndex != -1)
        {
            mainMeshRenderer.material.mainTexture = skins[skinIndex];
        }
        game = Game.Singleton;
        valueManager = ValueManager.Singleton;
        cubeSpawner = CubeSpawner.Singleton;
        body = GetComponent<Rigidbody>();
        currentTransform = transform;
        LeftWall = LeftWallRdr.material;
        RightWall = RightWallRdr.material;
        valueManager.CubeCountChanged += delegate
        {
            IEnumerator localMove()
            {
                yield return new WaitForSeconds(0.2f);
                Move(0f, false);
            }
            StartCoroutine(localMove());
            if (valueManager.GetCubeCount(false) == CubeSpawner.MaxCubeCount)
            {
                snowSystemEnabled = true;
                SnowSystem.Play();
            }
        };
        var tutorialManager = TutorialManager.shared;
        if (!tutorialManager.TutorialCompleted)
            yield return new WaitUntil(() => tutorialManager.TutorialCompleted);
        LeftMovePanel.SetActive(true);
        RightMovePanel.SetActive(true);
        effectTimeView.gameObject.SetActive(true);
        DisplayTargetColor();
        StartCoroutine(TimeEstimated());
    }
    private IEnumerator TimeEstimated() // корутина, отсчитывающая время до "доступности" эффекта
    {
        var delay = new WaitForSeconds(1f);
        for (timeForEffect = 30; timeForEffect > 0; timeForEffect--)
        {
            effectTimeView.text = timeForEffect.ToString();
            yield return delay;
        }
        effectTimeView.text = LocalizeManager.GetLocalizedString(Translation.TapHereToGoThroughAnyCube, true);
    }
    private void Move(float x, bool changePosIndex = true) // передвижение шарика
    {
        var currentXPos = raceXPos[valueManager.GetCubeCount(false)];
        if (changePosIndex)
            posIndex = Mathf.Clamp(posIndex + (x < 0f ? -1 : 1), 0, currentXPos.Length - 1);
        var target = new Vector3(currentXPos[posIndex], 0.175f, 3.01f);
        IEnumerator Moving()
        {
            isMoving = true;
            var lerp = 0f;
            for (int i = 0; i < 10; i++)
            {
                lerp += 0.1f;
                body.MovePosition(Vector3.Lerp(body.position, target, lerp));
                yield return new WaitForFixedUpdate();
            }
            isMoving = false;
        }
        if (isMoving)
            StopCoroutine(Moving());
        StartCoroutine(Moving());
    }
    public void SetTimeTextActive(bool active) // включение/выключение текста отсчёта
    {
        effectTimeViewGameObject.SetActive(active);
    }
    public void StartInvisibleAndTriigerEffect() // запуск эффекта
    {
        if (timeForEffect == 0 && game.State == GameState.Running)
        {
            SetTimeTextActive(false);
            mainMeshRenderer.material.color = effectColor;
            isInEffect = true;
            mainCollider.isTrigger = true;
        }
    }
    private void DisplayTargetColor() // отображение цвета, кубик в цвете которого нужно сбить
    {
        var color = valueManager.TargetColor;
        LeftWall.color = color;
        RightWall.color = color;
        if (snowSystemEnabled)
            SnowSystem.startColor = color;
    }
    private void FixedUpdate() // вращение вокруг своей оси
    {
        currentTransform.Rotate(50f * valueManager.GetCubeSpeed(false) * Vector3.left);
    }
    private void OnTriggerEnter(Collider other) // отслеживание столкновения с кубиком во время эффекта
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            SpawnCubes();
        }
    }
    private void SpawnCubes() // спавн кубиков
    {
        cubeSpawner.UpdateColor();
        DisplayTargetColor();
        cubeSpawner.Spawn();
    }
    private void OnTriggerExit(Collider other) // окончание эффекта
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            mainMeshRenderer.material.color = white;
            isInEffect = false;
            mainCollider.isTrigger = false;
            SetTimeTextActive(true);
            StartCoroutine(TimeEstimated());
        }
    }
    private void OnCollisionEnter(Collision collision) // отслеживание столкновения в обычном режиме, завершение игры
    {
        var gameObject = collision.gameObject;
        var name = gameObject.name;
        if (gameObject.CompareTag("Cube"))
        {
            if (name.Contains("targetColor"))
            {
                DestroyCubeSound.Play();
                gameObject.GetComponent<CubeController>().DestroyCube(true);
                SpawnCubes();
                game.IncreaseDestroyedCubesCount(true);
            }
            else
            {
                game.GameOver();
            }
        }


    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartInvisibleAndTriigerEffect();
    }
#endif
    public void StartMove(float XDirection) // начало движения объекта
    {
        if (game.State == GameState.Running && CanMove)
            Move(Mathf.Clamp(XDirection, -1f, 1f));
    }
}
