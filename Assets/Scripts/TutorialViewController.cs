using UnityEngine;
using TMPro;
public class TutorialViewController : MonoBehaviour // контроллер представления туториала
{
    [SerializeField] private GameTutorialType gameTutorialType; // тип туториала
    private void Start() // локализация туториала
    {
        GetComponent<TextMeshProUGUI>().text = TutorialData.Shared.GetGameTutorialText(gameTutorialType); 
    }
    public void SetActiveFalse() // деактивация представления
    {
        gameObject.SetActive(false);
    }
}
