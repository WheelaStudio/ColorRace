using TMPro;
using UnityEngine;
public class ShopAlertController : MonoBehaviour // локализация подсказки о открывшейся возможности купить скин
{
    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = TutorialData.Shared.ShopAlertText; // получение локализованой подсказки
    }
}
