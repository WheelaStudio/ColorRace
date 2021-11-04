using UnityEngine;
public class CubeController : MonoBehaviour
{
    private Vector3 direction;
    private Rigidbody body;
    private Color particleColor;
    public float speed;
    [SerializeField] private GameObject particleSystemObj;
    [SerializeField] private ParticleSystem destroyEffect;
    private ValueManager valueManager;
    private void Start()
    {
        direction = Vector3.forward * speed;
        body = GetComponent<Rigidbody>();
        valueManager = ValueManager.Singleton;
    }
    private void FixedUpdate()
    {
        body.MovePosition(body.position + direction);
    }
    private void OnDestroy()
    {
        if (valueManager.LastDestroyCaller is BallController)
        {
            particleSystemObj.transform.parent = null;
            destroyEffect.startColor = particleColor;
            particleSystemObj.SetActive(true);
            particleSystemObj.GetComponent<ParticleController>().Init();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            valueManager.LastDestroyCaller = this;
            Destroy(gameObject);
        }
    }
    public void SetColor(Color color, bool instanceParticleColor)
    {
        var assignedColor = color;
        GetComponent<MeshRenderer>().material.color = assignedColor;
        if (instanceParticleColor)
            particleColor = assignedColor;
    }
}
