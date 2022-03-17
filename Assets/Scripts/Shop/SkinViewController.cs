using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
public class SkinViewController : MonoBehaviour // поворот скина в магазиние за счёт свайпов
{
    public static GameObject SharedGameObject { get; private set; } // ссылка на скин, доступная всем
    [SerializeField] private UIBehaviour acceptPanel; // ссылка на панель для подтверждения покупки
    private Transform currentTransform; // кэш трансформа
    private Vector3 one; // вектор (1,1,1)
    private void Awake() // иницализация полей
    {
        one = Vector3.one;
        currentTransform = transform;
        SharedGameObject = gameObject;
    }
    
    private void OnMouseDrag() // поворот скина
    {
        if (Input.touchCount > 0 && currentTransform.localScale == one && !acceptPanel.IsActive())
            currentTransform.Rotate(Input.touches.Last().deltaPosition.y * 0.3f,0f,0f);   
    }
    
}
