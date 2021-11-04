using TMPro;
using UnityEngine;
public class ShopAlertController : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = TutorialData.Shared.ShopAlertText;
    }
}
