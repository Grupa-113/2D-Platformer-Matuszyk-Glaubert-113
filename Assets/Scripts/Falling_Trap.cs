using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    [Header("Ustawienia")]
    public float detectionDistance = 10f; 
    public float gravityScale = 5f;       
    public LayerMask playerLayer;         
    public string groundTag;   

    private Rigidbody2D rb;
    private bool falling = false;
    private bool falled = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (falling) return;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, detectionDistance, playerLayer);
        if (hit.collider != null)
        {
            Fall();
        }
    }

    void Fall()
    {
        falling = true;
        rb.bodyType = RigidbodyType2D.Dynamic; 
        rb.gravityScale = gravityScale;        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.CompareTag(groundTag))
        {
            rb.bodyType = RigidbodyType2D.Static;
            GetComponent<Collider2D>().isTrigger = false;

            float gridSize = 0.16f;
            float snapY = Mathf.Round(transform.position.y / gridSize) * gridSize + 0.08f;

            transform.position = new Vector3(transform.position.x, snapY, transform.position.z);

            falled = true;
        }


        if (col.gameObject.CompareTag("Player") && !falled )
        {
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            player.Die();
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * detectionDistance);
    }
}