using UnityEngine;
using System.Collections.Generic;
using System;
public enum Language // языки
{
    English, Russian
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
    // id'шники переводов
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
    public static string GetLocalizedString(int id, bool isGameWords) // получить доступ к локализованной строке
    {
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
