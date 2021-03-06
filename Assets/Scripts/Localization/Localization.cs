using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Localization : MonoBehaviour // локализация статичного текста
{
    [Header("Translated text identificator")]
    [Tooltip("New record = 0\nPause = 1\nResume = 2\nGame Over = 3\nQuit = 4\nRestart = 5" +
        "\nTap here to go through any cube = 6\nGame tutorial = 7\nControl tutorial 8" +
        "\nMax time = 0\nMax score = 1\nBuy for = 2\nPlay = 3\nSkins = 4\nMoney = 5" +
        "\nAccept question = 6\nYes = 7\nNo = 8\nEquip = 9\nRemove = 10\nSettings = 11" +
        "\nLanguage word = 12\nRU = 13\nEN = 14\nQuit question = 15\nRestart question = 16" +
        "\nTime = 17\nScore = 18\nShop = 19\nLoading = 20\nFPSEnabled = 21\nClear data = 22\nRequestClearData = 23")]
    [SerializeField] private Translation id; // id перевода
    private TextMeshProUGUI shared; // текст
    private bool isMenu; // в меню ли находится текст
    private LocalizeManager.ChangeLanguageDelegate changeLanguageDelegate; // оповещение смены языка
    private void Start() // инициализация смены языка
    {
        isMenu = SceneManager.GetActiveScene().buildIndex == 0;
        shared = GetComponent<TextMeshProUGUI>();
        if (isMenu)
        {
            changeLanguageDelegate = delegate
            {
                shared.text = LocalizeManager.GetLocalizedString(id, !isMenu);
            };
            changeLanguageDelegate.Invoke();
            LocalizeManager.AddChangeListener(changeLanguageDelegate);
        }
        else if(LocalizeManager.CurrentLanguage == Language.Russian)
            shared.text = LocalizeManager.GetLocalizedString(id, !isMenu);
    }
    private void OnDestroy() // удаление оповещений из списка
    {
        if (isMenu && !LocalizeManager.IsChangeListenersListClear)
        {
            LocalizeManager.ClearChangeListeners();
        }
    }
}
