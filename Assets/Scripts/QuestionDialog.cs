using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class QuestionDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button Yes, No;
    [SerializeField] private GameObject dialogPanel;
    public void Show(string question, UnityAction yes, UnityAction no = null)
    {
        dialogPanel.SetActive(true);
        questionText.text = question;
        Yes.onClick.AddListener(yes);
        No.onClick.AddListener(no ?? delegate { Hide(); });
    }
    private void OnDisable()
    {
        Yes.onClick.RemoveAllListeners();
        No.onClick.RemoveAllListeners();
    }
    public void Hide()
    {
        dialogPanel.SetActive(false);
    }
}
