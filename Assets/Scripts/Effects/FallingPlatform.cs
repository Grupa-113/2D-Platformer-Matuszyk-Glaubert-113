using System.Collections;
using UnityEngine;
 
public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float delayToFallDown = 1.0f;
    [SerializeField] private float delayToDestroy = 2.0f;

    private bool falling = false;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float shakeDuration = 0.8f; // czas trzesienia krotszy niz spadanie w dol
    [SerializeField] private float shakeMagnitude = 0.01f;
   
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator ShakePlatform(float duration)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector2 offset = Random.insideUnitCircle * shakeMagnitude;
            transform.position = startPos + new Vector3(offset.x, offset.y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
    }

    private IEnumerator StartFalling()
    {
        falling = true;
        shakeDuration += delayToFallDown;
        yield return StartCoroutine(ShakePlatform(shakeDuration));

        float left = delayToFallDown - shakeDuration;
        if(left > 0.0f)
        {
            yield return new WaitForSeconds(delayToFallDown);
        }
        
    
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, delayToDestroy);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Player" && !falling)
        {
            StartCoroutine(StartFalling());
        }
    }

}