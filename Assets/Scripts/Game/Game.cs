using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public enum GameState // перечисление состояний игры
{
    Running, Paused, Losed
}
public class Game : MonoBehaviour // главный класс игры
{
    public static Game Singleton { get; private set; } // глобальная ссылка на скрипт
    private BallController ballController; // ссылка на шарик
    private TutorialManager tutorialManager; // ссылка на туториал
    [SerializeField] private Animator pausePanel; // аниматор панели паузы
    [SerializeField] private QuestionDialog questionDialog; // диалог подтверждения
    [SerializeField] private GameObject LosePanel, PausePanel, PauseButton, RecordMark, FPSText; // панель проигрыша, паузы, кнопка паузы, уведомления о рекорде, отображение fps
    [SerializeField] private TextMeshProUGUI timeText, scoreGameText, scoreGameOverText, moneyEarnedText; /* текста, отображающие время, в течение которого игрок 
 продержался, набранные очки в игре, при проигрыше, количество заработанных денег */
    private int destroyedCubesCount = 0; // количество уничтоженных кубиков
    private float time = 0f; // время, в течение которого игрок продержался
    public GameState State { get; private set; } = GameState.Running; // текущее состояние
    private void Awake() // инициализация глобальной ссылки
    {
        Singleton = this;
    }
    private IEnumerator Start() // иницализация остальных полей, ожидание окончания туториала 
    {
        ballController = BallController.Singleton;
        tutorialManager = TutorialManager.shared;
        if (Preferences.FPSViewEnabled)
            FPSText.SetActive(true);
        yield return new WaitUntil(() => tutorialManager.TutorialCompleted);
        PauseButton.SetActive(true);
    }
    private void Update() // счётчик времени, нажатие на кнопку "Назад"
    {
        if (State == GameState.Running && tutorialManager.TutorialCompleted)
            time += Time.deltaTime;
#if !UNITY_IOS
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(State == GameState.Running)
            {
                Pause();
            }
            else if (PausePanel.activeSelf && PausePanel.transform.localScale == Vector3.one)
            {
                OpenQuit();
            }
            else if (questionDialog.gameObject.activeSelf)
            {
                HideQuestionDialog();
            }
        }
#endif
    }
    private void OnApplicationFocus(bool focus) // пауза при потере фокуса на игре
    {
        if (!focus && State == GameState.Running && tutorialManager.TutorialCompleted)
            Pause();
    }
    public void IncreaseDestroyedCubesCount(bool show) // увеличение количества уничтоженных кубиков
    {
        destroyedCubesCount++;
        if (show)
            scoreGameText.text = destroyedCubesCount.ToString();
    }
    public void Pause() // пауза
    {
        Time.timeScale = 0f;
        if (ballController.IsTimeTextActive)
            ballController.SetTimeTextActive(false);
        PauseButton.SetActive(false);
        PausePanel.SetActive(true);
        pausePanel.Play("PausePanel");
        State = GameState.Paused;
    }
    public void Resume() // продолжение игры
    {
        Time.timeScale = 1f;
        if (!ballController.IsTimeTextActive && !ballController.IsInEffect)
            ballController.SetTimeTextActive(true);
        PauseButton.SetActive(true);
        PausePanel.SetActive(false);
        State = GameState.Running;
    }
    public void GameOver() // окончание игры
    {
        Time.timeScale = 0f;
#if !UNITY_STANDALONE && !UNITY_WEBGL
        Handheld.Vibrate();
#endif
        PauseButton.SetActive(false);
        ballController.SetTimeTextActive(false);
        LosePanel.SetActive(true);
        var time = Mathf.RoundToInt(this.time);
        string Seconds()
        {
            var seconds = time % 60;
            return seconds < 10 ? $"0{seconds}" : seconds.ToString();
        }
        timeText.text = $"{LocalizeManager.GetLocalizedString(Translation.Time, false)}{time / 60}:{ Seconds()}";
        scoreGameOverText.text = $"{LocalizeManager.GetLocalizedString(Translation.Score, false)}{destroyedCubesCount}";
        moneyEarnedText.text = $"+{destroyedCubesCount}";
        var oldTimeRecord = Preferences.TimeRecord;
        var oldScoreRecord = Preferences.ScoreRecord;
        var IsNewRecordGot = oldTimeRecord < time || oldScoreRecord < destroyedCubesCount;
        var tutorialData = TutorialData.Shared;
        var money = Preferences.Money;
        var purchasedSkins = Preferences.PurchasedSkins;
        var purchasedSkinsCount = 0;
        for (int i = 0; i < purchasedSkins.Length; i++)
            if (purchasedSkins[i])
                purchasedSkinsCount++;
        if (money < Skin.СheapestPrice && money + destroyedCubesCount >= Skin.СheapestPrice && !tutorialData.ShopAlertRequest && !tutorialData.ShopAlertDisplayed && purchasedSkinsCount == 0)
        {
            tutorialData.ShopAlertRequest = true;
            tutorialData.Save();
        }
        Preferences.SetMoney(destroyedCubesCount, true);
        if (IsNewRecordGot)
        {
            RecordMark.SetActive(true);
            if (oldTimeRecord < time)
                Preferences.TimeRecord = time;
            if (oldScoreRecord < destroyedCubesCount)
                Preferences.ScoreRecord = destroyedCubesCount;
        }
        State = GameState.Losed;
    }
    private void HideQuestionDialog()
    {
        questionDialog.Hide();
        PausePanel.SetActive(true);
    }
    public void OpenQuit() // открытие диалога выхода из игры
    {
        PausePanel.SetActive(false);
        questionDialog.Show(LocalizeManager.GetLocalizedString(Translation.QuitQuestion, false), delegate
        {
            Quit();
        }, delegate
        {
            HideQuestionDialog();
        });
    }
    public void OpenRestart() // открытие диалога рестарта игры
    {
        PausePanel.SetActive(false);
        questionDialog.Show(LocalizeManager.GetLocalizedString(Translation.RestartQuestion, false), delegate
        {
            Restart();
        }, delegate
        {
            HideQuestionDialog();
        });
    }
    private void Quit() // выход из игры
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    private void Restart() // перезапуск игры
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
