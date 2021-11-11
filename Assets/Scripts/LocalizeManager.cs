using UnityEngine;
using System.Collections.Generic;
using System;
public enum Language
{
    English, Russian
}
public static class LocalizeManager
{
    private const int MainWordsIndex = 0;
    private const int GameWordsIndex = 1;
    private const char WordsSeparator = ',';
    private const string RussianLocalizationDirectory = "Localization/Russian";
    private const string RussianLocalizationFileName = "RussianStaticWords";
    private const string EnglishLocalizationDirectory = "Localization/English";
    private const string EnglishLocalizationFileName = "EnglishStaticWords";
    private const string WordsTypeSeparator = "GAMEWORDS:";
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
    private static string[] englishStaticWords;
    private static string[] russianStaticWords;
    private static string[] russianGameStaticWords;
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
        var typeSeparator = new string[] { WordsTypeSeparator };
        var russianWords = Resources.Load<TextAsset>($"{RussianLocalizationDirectory}/{RussianLocalizationFileName}").text.Split(typeSeparator, StringSplitOptions.None);
        russianStaticWords = russianWords[MainWordsIndex].GetStringWithoutNewLines().Split(WordsSeparator);
        russianGameStaticWords = russianWords[GameWordsIndex].GetStringWithoutNewLines().Split(WordsSeparator);
        englishStaticWords = Resources.Load<TextAsset>($"{EnglishLocalizationDirectory}/{EnglishLocalizationFileName}").text.GetStringWithoutNewLines().Split(WordsSeparator);
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
