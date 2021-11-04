using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class MenuManager : MonoBehaviour
{
    private bool levelIsLoading = false;
    private TutorialData tutorialData;
    public static MenuManager Shared { get; private set; }
    [SerializeField]
    private GameObject SettingsPanel, shopAlert;
    private GameObject skinViewHolder;
    [SerializeField]
    private Animator canvasHandler, skinView;
    [SerializeField]
    private TextMeshProUGUI maxScore, maxTime;
    [SerializeField]
    private GameObject loadingText;
    [SerializeField]
    private Image VolumeSwitcher;
    [SerializeField]
    private Sprite SoundOn, SoundOff;
    private Settings settings;
    private void Awake()
    {
        Shared = this;
        LocalizeManager.Init();
        if(TutorialData.Shared is null)
        TutorialData.Load();
        tutorialData = TutorialData.Shared;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        if (!tutorialData.GameTutorialCompleted)
            SceneManager.LoadScene(1);
    }
    private void Start()
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
    private void UpdateRecords()
    {
        maxScore.text = $"{LocalizeManager.GetLocalizedString(LocalizeManager.MaxScore, false)}{Preferences.ScoreRecord}";
        var time = Preferences.TimeRecord;
        string Seconds()
        {
            var seconds = time % 60;
            return seconds < 10 ? $"0{seconds}" : seconds.ToString();
        }
        maxTime.text = $"{LocalizeManager.GetLocalizedString(LocalizeManager.MaxTime, false)}{time / 60}:{Seconds()}";
    }
    public void ChangeVolume()
    {
        var volume = AudioListener.volume == 0f ? 1f : 0f;
        AudioListener.volume = volume;
        VolumeSwitcher.sprite = volume == 1f ? SoundOn : SoundOff;
        Preferences.Volume = volume;
    }
    private IEnumerator LaunchGame()
    {
        yield return null;
        SceneManager.LoadSceneAsync(1);
    }

    public void Play()
    {
        if (shopAlert.activeSelf || settings.RequestPanelIsActive || levelIsLoading) return;
        levelIsLoading = true;
        loadingText.SetActive(true);
        StartCoroutine(LaunchGame());
    }
    public void OpenShop()
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
    public void ChangeStatsLocalization()
    {
        UpdateRecords();
    }
    public void OpenSettings()
    {
        if (!shopAlert.activeSelf && !settings.RequestPanelIsActive && !levelIsLoading)
            SettingsPanel.SetActive(true);
    }
}
