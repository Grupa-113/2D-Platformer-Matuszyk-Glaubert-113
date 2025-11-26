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


            isRunning = false;

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                isRunning = true;

                if (isFacingRight == false)
                {
                    Flip();
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
                isRunning = true;

                if (isFacingRight == true)
                {
                    Flip();
                }
            }

            Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 0.1f, false);

            animator.SetBool("isGrounded", IsGrounded());
            animator.SetBool("isRunning", isRunning);
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
            Debug.Log("Game over");
        }

        if (col.CompareTag("LevelFall")){
            this.transform.position = startPosition;
            GameManager.instance.AddLives(-1);
            Debug.Log("odjeto serduszko");
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
        if (col.CompareTag("Enemy"))
        {
            if(this.transform.position.y > col.gameObject.transform.position.y)
            {
                //GameManager.instance.AddenemiesKill(1);
                Debug.Log("Killed an enemy");
            }
            else {
                this.transform.position = startPosition;
                GameManager.instance.AddLives(-1);
                Debug.Log("odjeto serduszko");
            }
        }

    }
}
