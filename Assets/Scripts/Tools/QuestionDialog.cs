using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class QuestionDialog : MonoBehaviour // вопросительный диалог
{
    [SerializeField] private TextMeshProUGUI questionText; // текст с вопросм
    [SerializeField] private Button Yes, No; // кнопки да, нет
    [SerializeField] private GameObject dialogPanel; // панель
    public void Show(string question, UnityAction yes, UnityAction no = null) // показ диалога
    {
        dialogPanel.SetActive(true);
        questionText.text = question;
        Yes.onClick.AddListener(yes);
        No.onClick.AddListener(no ?? delegate { Hide(); });
    }
    private void OnDisable() // удаление всех листенеров при закрытии
    {
        Yes.onClick.RemoveAllListeners();
        No.onClick.RemoveAllListeners();
    }
    public void Hide() // закрытие диалога
    {
        dialogPanel.SetActive(false);
    }
}
