using System.Collections;
using UnityEngine;
public class BackGroundColorAnimation : MonoBehaviour
{
    private Camera cameraMain;
    private float colorH = 0f;
    private void Start()
    {
        cameraMain = GetComponent<Camera>();
        StartCoroutine(ColorChange());
    }
    private IEnumerator ColorChange()
    {
        var delay = new WaitForSeconds(0.1f);
        while (true)
        {
            while (colorH < 1f)
            {
                colorH += 0.01f;
                cameraMain.backgroundColor = Color.HSVToRGB(colorH, 0.5f, 1f);
                yield return delay;
            }
            while (colorH > 0f)
            {
                colorH -= 0.01f;
                cameraMain.backgroundColor = Color.HSVToRGB(colorH, 0.5f, 1f);
                yield return delay;
            }
        }
    }
}
