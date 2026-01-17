using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header( "Movement parameters" )]
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float jumpForce = 3.0f;

    private Rigidbody2D rb;    
    private Collider2D coll;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wall_To_JumpLayer;

    private const float rayLength = 0.175f;
    [SerializeField] private const float wallRayLength = 0.03f;

    private Animator animator;
    private bool isRunning = false;
    private bool isFacingRight=true;
    private Vector2 startPosition;

    private int horizontalInput;

    private bool stopPlayer = false;

    private bool wasGrounded = false;
    private float previousVelocityY;

    private float lastJumpTime; 
    private float jumpCooldown = 0.3f;

    private bool whileDie = false;
    private bool checkpointActivated = false;

    //WALL JUMP 
    [Header("Wall Jump System")]
    [SerializeField] private float wallSlidingSpeed = 2f; 
    [SerializeField] private Vector2 wallJumpPower = new Vector2(8f, 12f); 
    [SerializeField] private float wallJumpDuration = 0.2f;

    private bool isWallSliding;
    private bool isWallJumping;

    //WALL JUMP 
    [Header("Particles")]
    [SerializeField] private GameObject jumpEffect;
    [SerializeField] private float offsetJumpEffect = 0.1f;

    [SerializeField] private GameObject fallEffect;
    [SerializeField] private float offsetfallEffect = 0.1f;
    [SerializeField] private float minFallVelocity = -5f;
    [SerializeField] private float minFallToShakeVelocity = -10f;

    [SerializeField] private GameObject wallJumpEffect;
    [SerializeField] private float offsetWallJumpEffect = 0.1f;

    [SerializeField] private float bouncePower = 10.0f;

    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.01f;

    // sounds
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip endSound;
    [SerializeField] private AudioClip killedEnemy;
    [SerializeField] private AudioClip heartSound;
    [SerializeField] private AudioClip keySound;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioClip checkPointSound;

    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip landing;
    [SerializeField] private AudioClip lvlCompleted; 
    [SerializeField] private AudioClip gameOver;
    // skok,ladowanie,wallJump
    private AudioSource source;
    public AudioClip lvlCompletedSound;
    public AudioClip lvlNotCompletedSound;
    private void Awake()
    {
        startPosition = transform.position;
        rb =GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        source = GetComponent<AudioSource>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GAME && !animator.GetBool("isDead") && !stopPlayer)
        {
            HandleFallEfect();

           //inputy 
           horizontalInput = 0;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){   horizontalInput = 1;    }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){   horizontalInput = -1;   }
            
            //aktualizacja isWallSlide
            HandleWallSlide();

            //logika skakania 
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (isWallSliding)
                {
                    PerformWallJump();
                }
                else
                {
                    Jump();
                }
            }
            //odwracanie sprita gracza 
            if (!isWallJumping)
            {
                if (horizontalInput > 0 && !isFacingRight) Flip();
                else if (horizontalInput < 0 && isFacingRight) Flip();
            }

            //kontrola animacji 
            isRunning = horizontalInput != 0;
            animator.SetBool("isGrounded", IsGrounded());
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isWallSliding", isWallSliding);
        }
    }

    void FixedUpdate()
    {
        if (GameManager.instance.currentGameState == GameState.GAME && !stopPlayer)
        {
            //ruch platformy na której jest gracz  
            if (isWallSliding && !isWallJumping)//slizganie po scianie 
            {
                //jesli sie slizgamy po lewej scianie nie mozemy robic ruchu w lewo itd 
                if (isFacingRight && horizontalInput<0)
                {
                    rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
                }
                else if(!isFacingRight && horizontalInput>0)
                {
                    rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
                }
            }
            else if (!isWallJumping)
            {
                rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
            }
        }
    }


    private void HandleFallEfect() {

        bool isGrounded = IsGrounded();
        //wykrycie ladowania
        if (isGrounded && !wasGrounded && previousVelocityY < minFallVelocity)
        {
            Instantiate(fallEffect, new Vector3(transform.position.x, transform.position.y + offsetfallEffect, transform.position.z), Quaternion.identity);
        }

        if (isGrounded && !wasGrounded && previousVelocityY < minFallToShakeVelocity)
        {
            float shakeIntensity = Mathf.Abs(previousVelocityY) * shakeMagnitude;
            CameraController.instance.StartShake(shakeDuration, shakeIntensity);
        }
        wasGrounded = isGrounded;
        previousVelocityY = rb.linearVelocity.y;
    }


    private bool IsTouchingWall() //sprawdzanie czy dotykamy œciany
    {
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(coll.bounds.center, direction, wallRayLength + coll.bounds.extents.x, wall_To_JumpLayer);
        Debug.DrawRay(coll.bounds.center, direction * (wallRayLength + coll.bounds.extents.x), Color.blue);
        return hit.collider != null;
    }

    private void HandleWallSlide()
    { 
        bool isTouchingWall = IsTouchingWall();
        bool isGrounded = IsGrounded();

        if (isTouchingWall && !isGrounded )//wall sliding tylko jak NIE dotykamy ziemi i DOTYKAMY sciany  
        {
            isWallSliding = true;
            //sztuczne tarcie 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void PerformWallJump()
    {
        isWallJumping = true;

        // Reset prêdkoœci
        rb.linearVelocity = Vector2.zero;

        // Kierunek odbicia (przeciwny do patrzenia)
        float jumpDirection = isFacingRight ? -1 : 1;

        // skok
        Vector2 force = new Vector2(wallJumpPower.x * jumpDirection, wallJumpPower.y);
        rb.AddForce(force, ForceMode2D.Impulse);


        GameObject particle = Instantiate(wallJumpEffect, new Vector3(transform.position.x, transform.position.y + offsetWallJumpEffect, transform.position.z), Quaternion.identity);
        if (!isFacingRight)
        {
            Vector3 newScale = particle.transform.localScale;
            newScale.x *= -1; 
            particle.transform.localScale = newScale;
        }
        Flip();
        StartCoroutine(StopWallJumpAfterTime());
    }

    IEnumerator StopWallJumpAfterTime()
    {
        yield return new WaitForSeconds(wallJumpDuration);
        isWallJumping = false;
    }

    bool IsGrounded()
    {
        Bounds bounds = coll.bounds;

        float yPos = bounds.min.y + 0.05f;

        float xLeft = bounds.min.x;
        float xRight = bounds.max.x;

        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(xLeft, yPos), Vector2.down, rayLength, groundLayer.value);
        Debug.DrawRay(new Vector3( xLeft,transform.position.y,transform.position.z), Vector2.down * rayLength, Color.green);
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(xRight, yPos), Vector2.down, rayLength, groundLayer.value);
        Debug.DrawRay(new Vector3(xRight, transform.position.y, transform.position.z), Vector2.down * rayLength, Color.green);

        return hitLeft.collider != null || hitRight.collider != null;
    }

    public bool IsPlayerGrounded()
    {
        return IsGrounded();
    }

        void Jump()
    {
        if (IsGrounded() && Time.time - lastJumpTime > jumpCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Instantiate(jumpEffect, new Vector3(transform.position.x, transform.position.y + offsetJumpEffect, transform.position.z), Quaternion.identity);
            lastJumpTime = Time.time;
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
        if (col.CompareTag("EndGame")) {
            
            if (GameManager.instance.CheckLevelCompleted())
            {
                source.PlayOneShot(lvlCompletedSound, AudioListener.volume);
            }
            else
            {
                source.PlayOneShot(lvlNotCompletedSound, AudioListener.volume);
            }
        }

        if (col.CompareTag("LevelFall")) { 
            Die(); 
        }

        if (col.CompareTag("Bonus"))
        {
            GameManager.instance.AddPoints(1);
            col.gameObject.SetActive(false);
            source.PlayOneShot(coinSound, AudioListener.volume);
        }

        if (col.CompareTag("Heart"))
        {
            col.gameObject.SetActive(false);
            GameManager.instance.UpdateLives(1);
            source.PlayOneShot(heartSound, AudioListener.volume);
        }

        if (col.CompareTag("Key"))
        {
            col.gameObject.SetActive(false);
            GameManager.instance.AddKeys();
            source.PlayOneShot(keySound, AudioListener.volume);
        }

        if (col.CompareTag("Checkpoint")){
            CheckPointFlag checkpointFlag = col.GetComponent<CheckPointFlag>();

            if (checkpointFlag.turnedOn)
            {
                checkpointFlag.turnedOn = false;
                startPosition = new Vector2(col.transform.position.x, col.transform.position.y + 0.3f);
                source.PlayOneShot(checkPointSound, AudioListener.volume);
            }
        }

        if (col.CompareTag("MovingPlatform"))
        {
            if (col.gameObject.activeInHierarchy)
            {
                transform.SetParent(col.transform);
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("MovingPlatform"))
        {
            if (col.gameObject.activeInHierarchy)
            {
                transform.SetParent(null);
            }
        }
    }



    public void Die()
    {
        if (!whileDie)
        {
            source.PlayOneShot(dieSound, AudioListener.volume);
            StartCoroutine(KillOnAnimationEnd());
        };
    }
    IEnumerator KillOnAnimationEnd()
    {
        whileDie = true;
        animator.SetBool("isDead", true);

        rb.bodyType = RigidbodyType2D.Static;

        yield return new WaitForSeconds(0.5f);

        GameManager.instance.UpdateLives(-1);

        animator.SetBool("isDead", false);

        rb.bodyType = RigidbodyType2D.Dynamic;

        if (GameManager.instance.livesNumber != 0)
        {
            transform.position = startPosition;
        }
        whileDie = false;
    }


    public void StopPlayer()
    {
        rb.linearVelocity = Vector2.zero;

        stopPlayer = true;
        animator.SetBool("isGrounded", true);
        animator.SetBool("isRunning", false);
    }
    public void StartPlayer()
    {
        stopPlayer = false;
    }
    public bool GetFacingRight()
    {
        return isFacingRight;
    }

    public void BounceFromEnemy()
    {
        rb.linearVelocity = new Vector2 (rb.linearVelocityX,0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);    
    }
}


