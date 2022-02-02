using UnityEngine;
using System.Collections.Generic;
using System;
public enum Language // языки
{
    English, Russian
}
// id'шники переводов
public enum Translation
{
    NewRecord = 0,
    Pause = 1,
    Resume = 2,
    GameOver = 3,
    Quit = 4,
    Restart = 5,
    TapHereToGoThroughAnyCube = 6,
    GameTutorial = 7,
    ControlTutorial = 8,
    MaxTime = 0,
    MaxScore = 1,
    BuyFor = 2,
    Play = 3,
    Skins = 4,
    Money = 5,
    AcceptQuestion = 6,
    Yes = 7,
    No = 8,
    Equip = 9,
    Remove = 10,
    Settings = 11,
    LanguageWord = 12,
    QuitQuestion = 13,
    RestartQuestion = 14,
    Time = 15,
    Score = 16,
    Shop = 17,
    Loading = 18,
    FPSEnabled = 19,
    ClearData = 20,
    RequestClearData = 21
}
public static class LocalizeManager // локализация игры
{
    private const int MainWordsIndex = 0; // слова меню
    private const int GameWordsIndex = 1; // слова игры
    private const char WordsSeparator = ','; // разделитель слов в файлах
    private const string RussianLocalizationDirectory = "Localization/Russian"; // директория русской локализации
    private const string RussianLocalizationFileName = "RussianStaticWords"; // имя русской локализации
    private const string EnglishLocalizationDirectory = "Localization/English"; // директория английской локализации
    private const string EnglishLocalizationFileName = "EnglishStaticWords"; // имя английской локализации
    private const string WordsTypeSeparator = "GAMEWORDS:"; // разделитель слов на категории: меню и игра
    public delegate void ChangeLanguageDelegate(); // делегат, оповещающий о смене языка
    private static List<ChangeLanguageDelegate> OnChangeLanguages; // список делегатов
    private static string[] englishStaticWords; // список английских слов в меню
    private static string[] russianStaticWords; // список русских слов в меню
    private static string[] russianGameStaticWords; // список русских слов в игре
    private const string languageKey = "LANGUAGE_KEY"; // ключ для доступа значения из сохранённых настроек
    private static Language language; // текущий язык
    public static Language CurrentLanguage // свойство, возвращающее текущий язык
    {
        get
        {
            return language;
        }
    }
    private static void SaveLanguage() // сохранение языка в памяти устройства
    {
        PlayerPrefs.SetInt(languageKey, (int)language);
        PlayerPrefs.Save();
    }
    public static void Init() // инициализация слов, текущего языка
    {
        language = (Language)PlayerPrefs.GetInt(languageKey, Application.systemLanguage == SystemLanguage.Russian ? 1 : 0);
        OnChangeLanguages = new List<ChangeLanguageDelegate>();
        var typeSeparator = new string[] { WordsTypeSeparator };
        var russianWords = Resources.Load<TextAsset>($"{RussianLocalizationDirectory}/{RussianLocalizationFileName}").text.Split(typeSeparator, StringSplitOptions.None);
        russianStaticWords = russianWords[MainWordsIndex].GetStringWithoutNewLines().Split(WordsSeparator);
        russianGameStaticWords = russianWords[GameWordsIndex].GetStringWithoutNewLines().Split(WordsSeparator);
        englishStaticWords = Resources.Load<TextAsset>($"{EnglishLocalizationDirectory}/{EnglishLocalizationFileName}").text.GetStringWithoutNewLines().Split(WordsSeparator);
    }
    public static void ClearChangeListeners() // очистка списка делегатов
    {
        OnChangeLanguages.Clear();
    }
    public static bool IsChangeListenersListClear // пуст ли список делегатов
    {
        get
        {
            return OnChangeLanguages.Count == 0;
        }
    }
    public static void AddChangeListener(ChangeLanguageDelegate changeLanguageDelegate) // добавить делегат в список
    {
        OnChangeLanguages.Add(changeLanguageDelegate);
    }
    public static bool RemoveChangeListener(ChangeLanguageDelegate changeLanguageDelegate) // удалить делегат из списка
    {
        return OnChangeLanguages.Remove(changeLanguageDelegate);
    }
    public static string GetLocalizedString(Translation traslationId, bool isGameWords) // получить доступ к локализованной строке
    {
        int id = (int)traslationId;
        return language == Language.English ? (isGameWords ? "Tap here to go through any cube!" : englishStaticWords[id]) : (isGameWords ? (id > 6 ? russianStaticWords[id] : russianGameStaticWords[id]) : russianStaticWords[id]);
    }
    public static void ChangeLanguage(Language value) // изменение языка
    {
        language = value;
        SaveLanguage();
        foreach (var i in OnChangeLanguages)
            i?.Invoke();
    }
}
