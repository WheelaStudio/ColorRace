using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Localization : MonoBehaviour
{
    [Header("Translated text identificator")]
    [Tooltip("New record = 0\nPause = 1\nResume = 2\nGame Over = 3\nQuit = 4\nRestart = 5" +
        "\nTap here to go through any cube = 6\nGame tutorial = 7\nControl tutorial 8" +
        "\nMax time = 0\nMax score = 1\nBuy for = 2\nPlay = 3\nSkins = 4\nMoney = 5" +
        "\nAccept question = 6\nYes = 7\nNo = 8\nEquip = 9\nRemove = 10\nSettings = 11" +
        "\nLanguage word = 12\nRU = 13\nEN = 14\nQuit question = 15\nRestart question = 16" +
        "\nTime = 17\nScore = 18\nShop = 19\nLoading = 20\nFPSEnabled = 21\nClear data = 22\nRequestClearData = 23")]
    [SerializeField] private int id = -1;
    private TextMeshProUGUI shared;
    private bool isGame;
    private LocalizeManager.ChangeLanguageDelegate changeLanguageDelegate;
    private void Start()
    {
        isGame = SceneManager.GetActiveScene().buildIndex == 1;
        shared = GetComponent<TextMeshProUGUI>();
        if (!isGame)
        {
            changeLanguageDelegate = delegate
            {
                shared.text = LocalizeManager.GetLocalizedString(id, isGame);
            };
            if (LocalizeManager.CurrentLanguage == Language.Russian)
                changeLanguageDelegate.Invoke();
            LocalizeManager.AddChangeListener(changeLanguageDelegate);
        }
        else if (LocalizeManager.CurrentLanguage == Language.Russian)
            shared.text = LocalizeManager.GetLocalizedString(id, isGame);
    }
    private void OnDestroy()
    {
        if (!isGame && !LocalizeManager.IsChangeListenersListClear)
        {
            LocalizeManager.ClearChangeListeners();
        }
    }
}
