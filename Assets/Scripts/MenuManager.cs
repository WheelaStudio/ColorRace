using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class MenuManager : MonoBehaviour // меню игры
{
    private bool levelIsLoading = false; // загружен ли уровень с игрой
    private TutorialData tutorialData; // информация о туториале
    public static MenuManager Shared { get; private set; } // глобальная ссылка на объект
    [SerializeField]
    private GameObject SettingsPanel, shopAlert; // панель настроек, оповещение о возможности покупки скина
    private GameObject skinViewHolder; // "держатель" скина
    [SerializeField]
    private Animator canvasHandler, skinView; // аниматоры канваса, представления скина
    [SerializeField]
    private TextMeshProUGUI maxScore, maxTime; // представления рекордов
    [SerializeField]
    private GameObject loadingText; // текст с загрузкой
    [SerializeField]
    private Image VolumeSwitcher; // включение/выключение звука
    [SerializeField]
    private Sprite SoundOn, SoundOff; // спрайты "включенный/выключенный звук"
    private Settings settings; // настройки
    private void Awake() // иницализация полей, включение туториала
    {
        Shared = this;
        LocalizeManager.Init();
        if(TutorialData.Shared is null)
        TutorialData.Load();
        tutorialData = TutorialData.Shared;
        Application.targetFrameRate = Application.platform == RuntimePlatform.WebGLPlayer ? 60 : Screen.currentResolution.refreshRate;
        if (!tutorialData.GameTutorialCompleted)
            SceneManager.LoadScene(1);
    }
    private void Start() // иницализация остальных полей
    {
        skinViewHolder = SkinViewController.SharedGameObject;
        settings = SettingsPanel.GetComponent<Settings>();
        var volume = Preferences.Volume;
        AudioListener.volume = volume;
        VolumeSwitcher.sprite = volume == 1f ? SoundOn : SoundOff;
        UpdateRecords();
        if (!tutorialData.ShopAlertDisplayed && tutorialData.ShopAlertRequest)
        {
            shopAlert.SetActive(true);
            canvasHandler.Play("ShopAlert");
        }
    }
    private void UpdateRecords() // представление рекордов
    {
        maxScore.text = $"{LocalizeManager.GetLocalizedString(Translation.MaxScore, false)}{Preferences.ScoreRecord}";
        var time = Preferences.TimeRecord;
        string Seconds()
        {
            var seconds = time % 60;
            return seconds < 10 ? $"0{seconds}" : seconds.ToString();
        }
        maxTime.text = $"{LocalizeManager.GetLocalizedString(Translation.MaxTime, false)}{time / 60}:{Seconds()}";
    }
    public void ChangeVolume() // включить/выключить звук
    {
        var volume = AudioListener.volume == 0f ? 1f : 0f;
        AudioListener.volume = volume;
        VolumeSwitcher.sprite = volume == 1f ? SoundOn : SoundOff;
        Preferences.Volume = volume;
    }
    private IEnumerator LaunchGame() // запуск уровня с игрой
    {
        yield return null;
        SceneManager.LoadSceneAsync(1);
    }

    public void Play() // старт запуска уровня с игрой
    {
        if (shopAlert.activeSelf || settings.RequestPanelIsActive || levelIsLoading) return;
        levelIsLoading = true;
        loadingText.SetActive(true);
        StartCoroutine(LaunchGame());
    }
    public void OpenShop() // открытие магазина, выключение подсказки
    {
        if (!SettingsPanel.activeSelf && !settings.RequestPanelIsActive && !levelIsLoading)
        {
            if (tutorialData.ShopAlertRequest && shopAlert.activeSelf)
            {
                tutorialData.ShopAlertRequest = false;
                tutorialData.ShopAlertDisplayed = true;
                tutorialData.Save();
                    shopAlert.SetActive(false);
            }
            canvasHandler.Play("OpenShop");
            if (skinViewHolder.activeSelf)
                skinView.Play("OpenSkinView");
        }
    }
    public void ChangeStatsLocalization() // обновление представления рекордов со сменой локализации
    {
        UpdateRecords();
    }
    public void OpenSettings() // открытие настроек игры
    {
        if (!shopAlert.activeSelf && !settings.RequestPanelIsActive && !levelIsLoading)
            SettingsPanel.SetActive(true);
    }
}
