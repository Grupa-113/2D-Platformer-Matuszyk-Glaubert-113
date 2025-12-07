using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header( "Movement parameters" )]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float jumpForce = 3.0f;
    //[Space(10)];
    private Rigidbody2D rigidBody;
    [SerializeField] private LayerMask groundLayer;
    private const float rayLength = 0.25f;
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
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GAME)
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

            Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 0.1f, false);
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
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
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
        GameManager.instance.AddLives(-1);
        transform.position = startPosition;
        rigidBody.linearVelocity = Vector2.zero;
    }
}
