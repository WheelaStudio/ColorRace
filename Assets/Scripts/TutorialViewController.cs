using UnityEngine;
using TMPro;
public class TutorialViewController : MonoBehaviour
{
    [SerializeField] private GameTutorialType gameTutorialType;
    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = TutorialData.Shared.GetGameTutorialText(gameTutorialType);
    }
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
