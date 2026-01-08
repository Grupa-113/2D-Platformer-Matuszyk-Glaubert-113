using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float delayToFallDown = 1.0f; // za ile zacznie spadaæ po wejsciu gracza
    [SerializeField] private float delayToDestroy = 2.0f; // po jakim czasie znika

    private bool falling = false;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float shakeDuration = 0.8f; // jak d³ugo sie trzesie przed spadnieciem
    [SerializeField] private float shakeMagnitude = 0.01f;  
    [SerializeField] private float margin = 0.2f;

    [SerializeField] private bool respawn= true;
    [SerializeField] private float respawnDiffTime = 5.0f; // kiedy ma nowo sie zrespic

    [SerializeField] private Vector3 startPos;
    private Collider2D col;
    private TilemapRenderer tilemapRenderer;

    private void Awake()
    {
        
    }
    void Start()
    {
        startPos= transform.position;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col = GetComponent<Collider2D>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
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

        yield return new WaitForSeconds(delayToDestroy);

        if (respawn)
        {
            yield return StartCoroutine(Respawn());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Player" && !falling)
        {
            float playerBottom = col.collider.bounds.min.y;
            float platformTop = GetComponent<Collider2D>().bounds.max.y;

            if (playerBottom >= platformTop - margin)
            {
                StartCoroutine(StartFalling());
            }
        }
    }

    private IEnumerator Respawn()
    {
        col.enabled = false;
        if (tilemapRenderer)
        {
            tilemapRenderer.enabled = false;
        }

        yield return new WaitForSeconds(respawnDiffTime);

        transform.position = startPos;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        falling = false;
        col.enabled = true;

        if (tilemapRenderer)
        {
            tilemapRenderer.enabled = true;
        }
    }
}