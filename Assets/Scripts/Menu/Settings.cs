using UnityEngine;
using UnityEngine.UI;
public class Settings : MonoBehaviour // класс настроек
{
    [SerializeField] private QuestionDialog questionDialog; // вопросительный диалог
    [SerializeField] private GameObject[] Borders = new GameObject[2]; // окантовки кнопок переключения языка
    [SerializeField] private Toggle FPSToggle; // тумблер для включения/выключения показа фпс
    private MenuManager menuManager; // ссылка на MenuManager
    public bool RequestPanelIsActive // открыт ли вопросительный диалог
    {
        get
        {
            return questionDialog.gameObject.activeSelf;
        }
    }
    private void Start() // инициализация полей
    {
        menuManager = MenuManager.Shared;
        UpdateBorders((int)LocalizeManager.CurrentLanguage);
        FPSToggle.isOn = Preferences.FPSViewEnabled;
    }
    public void ChangeLocalize(int value) // смена языка
    {
        var language = value;
        LocalizeManager.ChangeLanguage((Language)language);
        menuManager.ChangeStatsLocalization();
        UpdateBorders(language);
    }
    public void SetDisplayFPSFlag(bool value) // включение/выключение показа фпс
    {
        Preferences.FPSViewEnabled = value;
    }
    private void UpdateBorders(int value) // обновление окантовок
    {
        var language = value;
        Borders[language].SetActive(true);
        Borders[language == 1 ? 0 : 1].SetActive(false);
    }
    public void Hide() // закрытие настроек 
    {
        gameObject.SetActive(false);
    }
    public void HideRequestPanel() // закрытие диалога
    {
        questionDialog.Hide();
        gameObject.SetActive(true);
    }
    public void RequestClearData() // запрос удаления всех настроек
    {
        Hide();
        questionDialog.Show(LocalizeManager.GetLocalizedString(Translation.RequestClearData, false), delegate
        {
            Preferences.ClearData();
#if UNITY_ANDROID
             AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("finish");
#else
            Application.Quit();
#endif
        }, delegate
        {
            HideRequestPanel();
        });
    }
}
