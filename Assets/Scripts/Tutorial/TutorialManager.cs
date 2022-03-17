using UnityEngine;
using System.Collections;
public class TutorialManager : MonoBehaviour // менеджер для показа туториала об игре 
{
    [SerializeField] private GameObject gameTutorial, controlTutorial, tutorialCanvas; // описание игры, описане управления игры, канвас с туториалом
    public static TutorialManager shared; // глобальная ссылка на проект
    private TutorialData tutorailData; // информация о туториале
    public bool TutorialCompleted { get; private set; } // закончен ли туториал
    private void Awake() // инициализация значений
    {
        shared = this;
        tutorailData = TutorialData.Shared;
        TutorialCompleted = tutorailData.GameTutorialCompleted;
        if (TutorialCompleted)
        {
            Destroy(tutorialCanvas);
            Destroy(gameObject);
        }
    }
    private void Start() // старт отображения
    {
        StartCoroutine(DisplayTutorial());
    }
    private IEnumerator DisplayTutorial() // отображение
    {
        gameTutorial.SetActive(true);
        yield return new WaitForSeconds(2f);
        controlTutorial.SetActive(true);
        yield return new WaitWhile(() => controlTutorial.activeSelf);
        Destroy(tutorialCanvas);
        TutorialCompleted = true;
        tutorailData.GameTutorialCompleted = TutorialCompleted;
        tutorailData.Save();
        Destroy(gameObject);
    }
#if !UNITY_STANDALONE
    private void Update() // удержание игры пальцем
    {
        if (Input.touchCount > 0 && Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
        }
        else if (Time.timeScale == 0f && Input.touchCount == 0f)
            Time.timeScale = 1f;
    }
#endif
}
