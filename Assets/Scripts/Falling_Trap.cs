using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    [Header("Ustawienia")]
    public float detectionDistance = 10f; 
    public float gravityScale = 5f;       
    public LayerMask playerLayer;         
    public string groundTag;   

    private Rigidbody2D rb;
    private Collider2D coll;
    private bool falling = false;
    public bool falled = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;

        coll.isTrigger = false;
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

    public void Fall()
    {
        coll.isTrigger = false;
        falling = true;
        rb.bodyType = RigidbodyType2D.Dynamic; 
        rb.gravityScale = gravityScale;
        rb.mass = 1000;
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        HandleCollision(col);
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        HandleCollision(col);
    }


    private void HandleCollision(Collision2D col)
    {
        FallingTrap otherTrap = col.gameObject.GetComponent<FallingTrap>();

        // hit w innego trapa "DOMINO"
        if (otherTrap != null && !falled)
        {
            if (!otherTrap.falled && !otherTrap.falling)
            {
                otherTrap.Fall();
            }
        }

        //jesli spadl to nic nie sprawdza
        if (falled) return;


        bool isJustGround = col.gameObject.CompareTag(groundTag) && otherTrap == null; //prawdziwa ziemia
        bool isFallenTrap = (otherTrap != null && otherTrap.falled);    //dotkciecie sie z trapem który ju¿ spad³ czyli jak ziemia 

        
        bool hitFromBottom = false; // czy dotkciecie jest od dolu 
        foreach (ContactPoint2D contact in col.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                hitFromBottom = true;
                break;
            }
        }

        //spadanie na cos stalego
        if ((isJustGround || isFallenTrap) && hitFromBottom)
        {
            rb.bodyType = RigidbodyType2D.Static;
            coll.isTrigger = false;
            falled = true;
            falling = false;

            
            if (isFallenTrap) //skrzynka na skrzynce
            {
                transform.position = new Vector3(transform.position.x, otherTrap.transform.position.y + 0.16f, transform.position.z);
            }
            else  //upadek na ziemie 
            {
                float gridSize = 0.16f;
                float snapY = Mathf.Floor((transform.position.y / gridSize) + 0.5f) * gridSize + 0.08f;
                transform.position = new Vector3(transform.position.x, snapY, transform.position.z);
            }
        }

       
        if (col.gameObject.CompareTag("Player"))
        {
            if (rb.linearVelocity.y < -1f)
            {
                PlayerController player = col.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.Die();
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * detectionDistance);
    }
}