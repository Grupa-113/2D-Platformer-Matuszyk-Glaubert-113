using UnityEngine;
using System.Collections;

public class BearEnemy : MonoBehaviour
{
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float followSpeed = 6.0f;
    [SerializeField] private float normalSpeed = 2.0f;
    [SerializeField] private float moveRange = 1.0f;
    [SerializeField] private float followRange = 8.0f;
    [SerializeField] private Transform player;

    private Collider2D coll;
    private float heightMargin = 0.05f;
    private bool isFacingRight = true;
    private bool isMovingRight = true;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private float startPositionX;
    private bool killed = false;
    private bool isFollowing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Awake()
    {

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPositionX = this.transform.position.x;
        coll = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (killed) { return; }

        float distX = Mathf.Abs(player.position.x - transform.position.x);

        // goni
        if (isFollowing)
        {
            moveSpeed = followSpeed;
            if (distX >= followRange || PlayerTooHigh())
            {
                isFollowing = false;
                moveSpeed = normalSpeed;
                Moving();
            }
            else
            {
                FollowPlayer();
            }
        }
        else
        {
            moveSpeed = normalSpeed;

            if (distX < followRange && HasGroundedBoth())
            {
                isFollowing = true;
                FollowPlayer();
            }
            else
            {
                Moving();
            }
        }


        

    }

    bool HasGroundedBoth()
    {
        Bounds bounds = coll.bounds;

        float groundY = bounds.min.y + 0.05f;
        Vector2 hitdown = new Vector2(transform.position.x, groundY);
        Vector2 dir;

        if(player.position.x >= transform.position.x)
        {
            dir = Vector2.right;
        }
        else
        {
            dir = Vector2.left;
        }

         RaycastHit2D hit = Physics2D.Raycast(hitdown, dir, followRange);


        if (hit.collider == null)
        {
            return false;
        }
            

        return hit.collider.CompareTag("Player");
    }

    bool PlayerTooHigh()
    {
        float bearTopY = coll.bounds.max.y;        
        float maxAllowedPlayerFootY = bearTopY + heightMargin;

        Collider2D playerColl = player.GetComponent<Collider2D>();
        float playerFootY;

        playerFootY = playerColl.bounds.min.y;   
   

        return playerFootY > maxAllowedPlayerFootY;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }

    void moveRight()
    {
        rigidBody.linearVelocity = new Vector2(moveSpeed, rigidBody.linearVelocity.y);
        //transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void moveLeft()
    {
        rigidBody.linearVelocity = new Vector2(-moveSpeed, rigidBody.linearVelocity.y);
        //transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    IEnumerator KillOnAnimationEnd()
    {
        /* yield return new WaitForSecond(time_in_seconds);*/
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("Player"))
        {
            if (this.transform.position.y < col.gameObject.transform.position.y)
            {
                killed = true;
                GetComponent<Collider2D>().enabled = false;
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
                GameManager.instance.AddenemiesKill(1);
            }
            else
            {
                PlayerController player = col.gameObject.GetComponent<PlayerController>();
                player.Die();
            }
        }
    }

    void FollowPlayer()
    {

        if (player.position.x > transform.position.x)
        {
            if (!isFacingRight)
            {
                Flip();
                
            }
            isMovingRight = true;
            moveRight();
        }
        else
        {
            if (isFacingRight)
            {
                Flip();
                
            }
            isMovingRight = false;
            moveLeft();
        }

    }

    private bool isWall()
    {
        Bounds bounds = coll.bounds;

        float groundY = bounds.min.y + 0.5f;
        Vector2 hitWall = new Vector2(transform.position.x, groundY);
        Vector2 dir;

        if (isMovingRight)
        {
            dir = Vector2.right;
        }
        else
        {
            dir = Vector2.left;
        }

        RaycastHit2D hit = Physics2D.Raycast(hitWall, dir, 0.05f);


        if (hit.collider == null)
        {
            return false;
        }


        return hit.collider.CompareTag("Platform");
    }

    void Moving()
    {
        if (isWall())
        {
            isMovingRight = !isMovingRight;
            Flip();
            return;
        }

        if (isMovingRight)
        {
            if (this.transform.position.x < startPositionX + moveRange)
            {
                moveRight();
            }
            else
            {
                isMovingRight = false;
                Flip();
            }
        }
        else
        {
            if (this.transform.position.x > startPositionX - moveRange)
            {
                moveLeft();
            }
            else
            {
                isMovingRight = true;
                Flip();
            }
        }
    }
    }