using UnityEngine;
public class ParticleController : MonoBehaviour
{
    private Vector3 direction;
    private Transform currentTransform;
    public void Init()
    {
        enabled = true;
        currentTransform = transform;
        direction = 0.16f * ValueManager.Singleton.GetCubeSpeed(false) * Vector3.forward;
        Destroy(gameObject, 1f);
    }
    private void FixedUpdate()
    {
        currentTransform.Translate(direction);
    }
}
