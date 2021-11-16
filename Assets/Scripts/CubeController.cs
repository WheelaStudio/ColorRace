using UnityEngine;
public class CubeController : MonoBehaviour
{
    private Vector3 direction;
    private Rigidbody body;
    private Color particleColor;
    public float speed;
    [SerializeField] private GameObject particleSystemObj;
    [SerializeField] private ParticleSystem destroyEffect;
    private void Start()
    {
        direction = Vector3.forward * speed;
        body = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        body.MovePosition(body.position + direction);
    }
    public void DestroyCube(bool enableParticle)
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
    private void OnBecameInvisible()
    {
            DestroyCube(false);    
    }
    public void SetColor(Color color, bool instanceParticleColor)
    {
        var assignedColor = color;
        GetComponent<MeshRenderer>().material.color = assignedColor;
        if (instanceParticleColor)
            particleColor = assignedColor;
    }
}
