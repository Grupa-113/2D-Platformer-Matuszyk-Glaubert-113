using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header( "Movement parameters" )]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float jumpForce = 3.0f;
    //[Space(10)];
    private Rigidbody2D rigidBody;
    
    private Collider2D coll;
 


    [SerializeField] private LayerMask groundLayer;
    private const float rayLength = 0.15f;
    private Animator animator;
    private bool isRunning = false;
    private bool isFacingRight=true;
    private Vector2 startPosition;

    private float horizontalInput;

    private void Awake()
    {
        startPosition = transform.position;
        rigidBody =GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GAME && !animator.GetBool("isDead"))
        {

            horizontalInput = 0;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                horizontalInput = 1;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                horizontalInput = -1;
            }




            isRunning = false;

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (horizontalInput > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (horizontalInput < 0 && isFacingRight)
            {
                Flip();
            }

            


            isRunning = horizontalInput != 0;
            animator.SetBool("isGrounded", IsGrounded());
            animator.SetBool("isRunning", isRunning);

        }
    }

    void FixedUpdate()
    {
        if (GameManager.instance.currentGameState == GameState.GAME)
        {
            rigidBody.linearVelocity = new Vector2(horizontalInput * moveSpeed, rigidBody.linearVelocity.y);
        }
    }



    bool IsGrounded()
    {
        Bounds bounds = coll.bounds;

        float yPos = bounds.min.y + 0.05f;

        float xLeft = bounds.min.x + 0.11f;
        float xRight = bounds.max.x - 0.11f;

        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(xLeft, yPos), Vector2.down, rayLength, groundLayer.value);
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(xRight, yPos), Vector2.down, rayLength, groundLayer.value);


        return hitLeft.collider != null || hitRight.collider != null;
    }


    void Jump()
    {
        if (IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("LevelExit"))
        {
            GameManager.instance.LevelCompleted();
            Debug.Log("Game over");
        }

        if (col.CompareTag("LevelFall")){
            Die();
        }

        if (col.CompareTag("Bonus") )
        {
            GameManager.instance.AddPoints(3);
            col.gameObject.SetActive(false);
        }
        if (col.CompareTag("Heart"))
        {
            col.gameObject.SetActive(false);
            GameManager.instance.AddLives(1);
            Debug.Log("dodano serduszko");
            
        }

    }

    public void Die()
    {
        StartCoroutine(KillOnAnimationEnd());
    }
    IEnumerator KillOnAnimationEnd()
    {
        animator.SetBool("isDead", true);
        rigidBody.bodyType = RigidbodyType2D.Static;

        yield return new WaitForSeconds(0.5f);

        GameManager.instance.AddLives(-1);

        animator.SetBool("isDead", false);
        rigidBody.bodyType = RigidbodyType2D.Dynamic;

        transform.position = startPosition;

       
        
    }
}
