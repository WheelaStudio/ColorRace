using UnityEngine;
using System.Collections;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject gameTutorial, controlTutorial, tutorialCanvas;
    public static TutorialManager shared;
    private TutorialData tutorailData;
    public bool TutorialCompleted { get; private set; }
    private void Awake()
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
    private void Start()
    {
        StartCoroutine(DisplayTutorial());
    }
    private IEnumerator DisplayTutorial()
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

    private void Update()
    {
        if (Input.touchCount > 0 && Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
        }
        else if (Time.timeScale == 0f && Input.touchCount == 0f)
            Time.timeScale = 1f;
    }
}
