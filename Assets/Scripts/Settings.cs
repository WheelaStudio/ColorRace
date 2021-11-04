using UnityEngine;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    [SerializeField] private QuestionDialog questionDialog;
    [SerializeField] private GameObject[] Borders = new GameObject[2];
    [SerializeField] private Toggle FPSToggle;
    private MenuManager menuManager;
    public bool RequestPanelIsActive
    {
        get
        {
            return questionDialog.gameObject.activeSelf;
        }
    }
    private void Start()
    {
        menuManager = MenuManager.Shared;
        UpdateBorders((int)LocalizeManager.CurrentLanguage);
        FPSToggle.isOn = Preferences.FPSViewEnabled;
    }
    public void ChangeLocalize(int value)
    {
        var language = value;
        LocalizeManager.ChangeLanguage((Language)language);
        menuManager.ChangeStatsLocalization();
        UpdateBorders(language);
    }
    public void SetDisplayFPSFlag(bool value)
    {
        Preferences.FPSViewEnabled = value;
    }
    private void UpdateBorders(int value)
    {
        var language = value;
        Borders[language].SetActive(true);
        Borders[language == 1 ? 0 : 1].SetActive(false);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void RequestClearData()
    {
        Hide();
        questionDialog.Show(LocalizeManager.GetLocalizedString(LocalizeManager.RequestClearData, false), delegate
        {
            Preferences.ClearData();
#if UNITY_IOS
            Application.Quit();
#else
AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
activity.Call("finish");
#endif
        }, delegate
        {
            questionDialog.Hide();
            gameObject.SetActive(true);
        });
    }
}
