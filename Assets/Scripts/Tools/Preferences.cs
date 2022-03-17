using UnityEngine;
using System;
public static class Preferences // класс для сохранения данных игры
{
    public const int SkinsCount = 5; // максимальное количество купленных скинов
    // ключи для доступа к значениям
    private const string TutorialDataKey = "TUTORIAL_DATA_KEY";
    private const string TimeRecordKey = "TIME_RECORD_KEY";
    private const string ScoreRecordKey = "SCORE_RECORD_KEY";
    private const string VolumeKey = "VOLUME_KEY";
    private const string MoneyKey = "MONEY_KEY";
    private const string EquipedSkinKey = "EQUIPEDSKIN_KEY";
    private const string PurchasedSkinsKey = "PURCHASEDSKINS_KEY";
    private const string FPSViewEnabledKey = "FPS_ENABLED_KEY";
    public static bool FPSViewEnabled // включен ли fps view
    {
        get
        {
            return Convert.ToBoolean(PlayerPrefs.GetInt(FPSViewEnabledKey));
        }
        set
        {
            PlayerPrefs.SetInt(FPSViewEnabledKey, Convert.ToInt32(value));
            PlayerPrefs.Save();
        }
    }
    public static TutorialData TutorialData // информация о туториале
    {
        get
        {
            return JsonUtility.FromJson<TutorialData>(PlayerPrefs.GetString(TutorialDataKey));
        }
        set
        {
            PlayerPrefs.SetString(TutorialDataKey, JsonUtility.ToJson(value));
            PlayerPrefs.Save();
        }
    }
    public static bool[] PurchasedSkins // купленные скины
    {
        get
        {
            var source = PlayerPrefs.GetString(PurchasedSkinsKey, "none");
            if (source == "none")
                return new bool[SkinsCount];
            var values = source.Split(',');
            var result = new bool[SkinsCount];
            for (int i = 0; i < SkinsCount; i++)
                result[i] = Convert.ToBoolean(values[i]);
            return result;
        }
        set
        {
            var prefs = "";
            var temp = SkinsCount - 1;
            for (int i = 0; i < SkinsCount; i++)
            {
                prefs += value[i].ToString();
                if (i < temp)
                    prefs += ",";
            }
            PlayerPrefs.SetString(PurchasedSkinsKey, prefs);
            PlayerPrefs.Save();
        }
    }
    public static int Money // деньги
    {
        get
        {
            return PlayerPrefs.GetInt(MoneyKey);
        }
    }
    public static void SetMoney(int value, bool increase = false) // установить количество денег
    {
        PlayerPrefs.SetInt(MoneyKey, increase ? Money + value : value);
        PlayerPrefs.Save();
    }
    public static int EquipedSkin // одетый скин
    {
        get
        {
            return PlayerPrefs.GetInt(EquipedSkinKey, -1);
        }
        set
        {
            PlayerPrefs.SetInt(EquipedSkinKey, value);
            PlayerPrefs.Save();
        }
    }
    public static int TimeRecord // рекорд по времнеи 
    {
        get
        {
            return PlayerPrefs.GetInt(TimeRecordKey);
        }
        set
        {
            PlayerPrefs.SetInt(TimeRecordKey, value);
            PlayerPrefs.Save();
        }
    }
    public static int ScoreRecord // рекорд по очкам
    {
        get
        {
            return PlayerPrefs.GetInt(ScoreRecordKey);
        }
        set
        {
            PlayerPrefs.SetInt(ScoreRecordKey, value);
            PlayerPrefs.Save();
        }
    }
    public static float Volume // громкость звука
    {
        get
        {
            return PlayerPrefs.GetFloat(VolumeKey, 1f);
        }
        set
        {
            PlayerPrefs.SetFloat(VolumeKey, value);
            PlayerPrefs.Save();
        }
    }
    public static void ClearData() // сброс всех настроек
    {
        PlayerPrefs.DeleteAll();
    }
}
