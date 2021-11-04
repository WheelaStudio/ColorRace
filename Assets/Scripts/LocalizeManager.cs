using UnityEngine;
using System.Collections.Generic;
public enum Language
{
    English, Russian
}
public static class LocalizeManager
{
    public const int NewRecord = 0;
    public const int Pause = 1;
    public const int Resume = 2;
    public const int GameOver = 3;
    public const int Quit = 4;
    public const int Restart = 5;
    public const int TapHereToGoThroughAnyCube = 6;
    public const int GameTutorial = 7;
    public const int ControlTutorial = 8;
    public const int MaxTime = 0;
    public const int MaxScore = 1;
    public const int BuyFor = 2;
    public const int Play = 3;
    public const int Skins = 4;
    public const int Money = 5;
    public const int AcceptQuestion = 6;
    public const int Yes = 7;
    public const int No = 8;
    public const int Equip = 9;
    public const int Remove = 10;
    public const int Settings = 11;
    public const int LanguageWord = 12;
    public const int RU = 13;
    public const int EN = 14;
    public const int QuitQuestion = 15;
    public const int RestartQuestion = 16;
    public const int Time = 17;
    public const int Score = 18;
    public const int Shop = 19;
    public const int Loading = 20;
    public const int FPSEnabled = 21;
    public const int ClearData = 22;
    public const int RequestClearData = 23;
    public delegate void ChangeLanguageDelegate();
    private static List<ChangeLanguageDelegate> OnChangeLanguages;
    private readonly static string[] englishStaticWords = { "Max time: ", "Max score: ", "Buy for ", "Play", "Skins", "Money", "Do you really wanna buy this?", "Yes"
            , "No", "Equip", "Remove", "Settings", "Language: ", "RU", "EN","Do you really wanna quit?", "Do you really wanna restart?","Time: ", "Score: ", "Shop", "loading...", "Display FPS", "Clear game data", "Do you really wanna clear game data?\nRestart the game after reset"};
    private readonly static string[] russianStaticWords = { "Макс. время: ", "Макс. очки: ", "Купить за ", "Играть", "Скины", "Монеты", "Вы действительно хотите купить это?", "Да",
        "Нет", "Надеть", "Снять", "Настройки","Язык игры: ", "РУС", "АНГ", "Вы действительно хотите выйти?", "Вы действительно хотите перезапустить игру?", "Время: ", "Очки: ", "Магазин", "загрузка..." ,"Отображать FPS", "Сбросить данные игры", "Вы действительно хотите сбросить данные игры?\nПерезапустите игру после сброса"};
    private readonly static string[] russianGameStaticWords = { "Новый рекорд!", "Пауза", "Продолжить", "Поражение", "Выход", "Рестарт", "Для прохода любого кубика насквозь нажмите здесь!" };
    private const string languageKey = "LANGUAGE_KEY";
    private static Language language;
    public static Language CurrentLanguage
    {
        get
        {
            return language;
        }
        set
        {
            language = value;
            SaveLanguage();
        }
    }
    private static void SaveLanguage()
    {
        PlayerPrefs.SetInt(languageKey, (int)language);
        PlayerPrefs.Save();
    }
    public static void Init()
    {
        language = (Language)PlayerPrefs.GetInt(languageKey, Application.systemLanguage == SystemLanguage.Russian ? 1 : 0);
        OnChangeLanguages = new List<ChangeLanguageDelegate>();
    }
    public static void ClearChangeListeners()
    {
        OnChangeLanguages.Clear();
    }
    public static bool IsChangeListenersListClear
    {
        get
        {
            return OnChangeLanguages.Count == 0;
        }
    }
    public static void AddChangeListener(ChangeLanguageDelegate changeLanguageDelegate)
    {
        OnChangeLanguages.Add(changeLanguageDelegate);
    }
    public static bool RemoveChangeListener(ChangeLanguageDelegate changeLanguageDelegate)
    {
        return OnChangeLanguages.Remove(changeLanguageDelegate);
    }
    public static string GetLocalizedString(int id, bool isGameWords)
    {
        return language == Language.English ? (isGameWords ? "Tap here to go through any cube!" : englishStaticWords[id]) : (isGameWords ? (id > 6 ? russianStaticWords[id] : russianGameStaticWords[id]) : russianStaticWords[id]);
    }
    public static void ChangeLanguage(Language value)
    {
        language = value;
        SaveLanguage();
        foreach (var i in OnChangeLanguages)
            i?.Invoke();
    }
}
