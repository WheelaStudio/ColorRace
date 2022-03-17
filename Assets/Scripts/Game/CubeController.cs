using UnityEngine;
public class CubeController : MonoBehaviour // контроль кубика
{
    private Vector3 direction; // направление движения
    private Rigidbody body; // ригидбоди
    private Color particleColor; // цвет партиклей после уничтожения
    public float speed; // скорость движения
    [SerializeField] private GameObject particleSystemObj;
    [SerializeField] private ParticleSystem destroyEffect;
    private void Start()
    {
        direction = Vector3.forward * speed; // расчёт скорости
        body = GetComponent<Rigidbody>(); // инициализация ригидбоди
    }
    private void FixedUpdate() // движение кубика
    {
        body.MovePosition(body.position + direction);
    }
    public void DestroyCube(bool enableParticle) // уничтожение кубика, включение партиклей
    {
        if (enableParticle)
        {
            particleSystemObj.transform.parent = null;
            destroyEffect.startColor = particleColor;
            particleSystemObj.SetActive(true);
            particleSystemObj.GetComponent<ParticleController>().Init();
        }
        Destroy(gameObject);
    }
    private void OnBecameInvisible() // уничтожение кубика, когда он пропадает из зоны видимости камеры
    {
            DestroyCube(false);    
    }
    public void SetColor(Color color, bool instanceParticleColor) // установка цвета партиклся
    {
        var assignedColor = color;
        GetComponent<MeshRenderer>().material.color = assignedColor;
        if (instanceParticleColor)
            particleColor = assignedColor;
    }
}
