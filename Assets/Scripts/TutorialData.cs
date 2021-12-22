using System;
public enum GameTutorialType // тип игрового туториала
{
    Description, Control
}
[Serializable]
public class TutorialData // описание данных туториала
{
    public bool GameTutorialCompleted = false; // завершён ли туториал об игре
    public bool ShopAlertRequest = false; // требуется ли показ подсказки о появившейся возможности купить скин
    public bool ShopAlertDisplayed = false; // показана ли подсказка о появившейся возможности купить скин
    public static TutorialData Shared { get; private set; } // глобальная ссылка на скрипт
    public static void Load() // загрузка данных
    {
        Shared = Preferences.TutorialData;
        if (Shared == null)
            Shared = new TutorialData();
    }
    public void Save() // сохранение данных
    {
        Preferences.TutorialData = Shared;
    }
    public string GetGameTutorialText(GameTutorialType gameTutorial) // получить локализованый туториал
    {
        if (gameTutorial == GameTutorialType.Control)
            return LocalizeManager.CurrentLanguage == Language.Russian ? "Для перемещения налево нажимать на левую часть экрана, направо - на правую" :
                "To move to the left, click on the left side of the screen, to the right - on the right";
        else
            return LocalizeManager.CurrentLanguage == Language.Russian ? "Цель игры -разбивать кубики в цвет бортиков стен" :
                "The goal of the game is to break the cubes in the color of the sides of the walls";
    }
    public string ShopAlertText // получить локализованую подскащу
    {
        get
        {
            return LocalizeManager.CurrentLanguage == Language.Russian ? "Перейдите в магазин для покупки своего первого скина!" :
                "Open the shop to buy your first skin!";
        }
    }
}
