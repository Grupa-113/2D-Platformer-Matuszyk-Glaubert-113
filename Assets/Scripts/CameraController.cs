using System.Collections;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
public class CameraController : MonoBehaviour
{
  
    public static CameraController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StartShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // U¿ywamy originalPosition.z, aby kamera nie "uciek³a" w g³¹b ekranu (2D)
            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
