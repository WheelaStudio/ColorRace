using UnityEngine;
public class ParticleController : MonoBehaviour // контроль системы партиклей после уничтожения кубика
{
    private Vector3 direction; // направление движения системы партиклей
    private Transform currentTransform; // трансформ
    public void Init() // инициализация
    {
        enabled = true;
        currentTransform = transform;
        direction = 0.16f * ValueManager.Singleton.GetCubeSpeed(false) * Vector3.forward;
        Destroy(gameObject, 1f);
    }
    private void FixedUpdate() // перемещение
    {
        currentTransform.Translate(direction);
    }
}
