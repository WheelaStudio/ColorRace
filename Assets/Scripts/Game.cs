using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public enum GameState
{
    Running, Paused, Losed
}
public class Game : MonoBehaviour
{
    public static Game Singleton { get; private set; }
    private BallController ballController;
    private TutorialManager tutorialManager;
    [SerializeField] private Animator pausePanel;
    [SerializeField] private QuestionDialog questionDialog;
    [SerializeField] private GameObject LosePanel, PausePanel, PauseButton, RecordMark, FPSText;
    [SerializeField] private TextMeshProUGUI timeText, scoreGameText, scoreGameOverText, moneyEarnedText;
    private int destroyedCubesCount = 0;
    private float time = 0f;
    public GameState State { get; private set; } = GameState.Running;
    private void Awake()
    {
        Singleton = this;
    }
    private IEnumerator Start()
    {
        ballController = BallController.Singleton;
        tutorialManager = TutorialManager.shared;
        if (Preferences.FPSViewEnabled)
            FPSText.SetActive(true);
        yield return new WaitUntil(() => tutorialManager.TutorialCompleted);
        PauseButton.SetActive(true);
    }
    private void Update()
    {
        if (State == GameState.Running && tutorialManager.TutorialCompleted)
            time += Time.deltaTime;
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus && State == GameState.Running && tutorialManager.TutorialCompleted)
            Pause();
    }
    public void IncreaseDestroyedCubesCount(bool show)
    {
        destroyedCubesCount++;
        if (show)
            scoreGameText.text = destroyedCubesCount.ToString();
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        if (ballController.IsTimeTextActive)
            ballController.SetTimeTextActive(false);
        PauseButton.SetActive(false);
        PausePanel.SetActive(true);
        pausePanel.Play("PausePanel");
        State = GameState.Paused;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        if (!ballController.IsTimeTextActive && !ballController.IsInEffect)
            ballController.SetTimeTextActive(true);
        PauseButton.SetActive(true);
        PausePanel.SetActive(false);
        State = GameState.Running;
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
#if !UNITY_EDITOR
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
        timeText.text = $"{LocalizeManager.GetLocalizedString(LocalizeManager.Time, false)}{time / 60}:{ Seconds()}";
        scoreGameOverText.text = $"{LocalizeManager.GetLocalizedString(LocalizeManager.Score, false)}{destroyedCubesCount}";
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
    public void OpenQuit()
    {
        PausePanel.SetActive(false);
        questionDialog.Show(LocalizeManager.GetLocalizedString(LocalizeManager.QuitQuestion, false), delegate
        {
            Quit();
        }, delegate
        {
            questionDialog.Hide(); PausePanel.SetActive(true);
        });
    }
    public void OpenRestart()
    {
        PausePanel.SetActive(false);
        questionDialog.Show(LocalizeManager.GetLocalizedString(LocalizeManager.RestartQuestion, false), delegate
        {
            Restart();
        }, delegate
        {
            questionDialog.Hide(); PausePanel.SetActive(true);
        });
    }
    private void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    private void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
