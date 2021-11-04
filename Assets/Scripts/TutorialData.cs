using System;
public enum GameTutorialType
{
    Description, Control
}
[Serializable]
public class TutorialData
{
    public bool GameTutorialCompleted = false;
    public bool ShopAlertRequest = false;
    public bool ShopAlertDisplayed = false;
    public static TutorialData Shared { get; private set; }
    public static void Load()
    {
        Shared = Preferences.TutorialData;
        if (Shared == null)
            Shared = new TutorialData();
    }
    public void Save()
    {
        Preferences.TutorialData = Shared;
    }
    public string GetGameTutorialText(GameTutorialType gameTutorial)
    {
        if (gameTutorial == GameTutorialType.Control)
            return LocalizeManager.CurrentLanguage == Language.Russian ? "Для перемещения налево нажимать на левую часть экрана, направо - на правую" :
                "To move to the left, click on the left side of the screen, to the right - on the right";
        else
            return LocalizeManager.CurrentLanguage == Language.Russian ? "Цель игры -разбивать кубики в цвет бортиков стен" :
                "The goal of the game is to break the cubes in the color of the sides of the walls";
    }
    public string ShopAlertText
    {
        get
        {
            return LocalizeManager.CurrentLanguage == Language.Russian ? "Перейдите в магазин для покупки своего первого скина!" :
                "Open the shop to buy your first skin!";
        }
    }
}
