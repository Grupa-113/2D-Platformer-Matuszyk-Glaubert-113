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

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;

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

            if (isWall())
            {
                isFollowing = false;
                moveSpeed = normalSpeed;
                isMovingRight = !isMovingRight; 
                Flip();
                Moving(); 
            }
            else if (distX >= followRange || PlayerTooHigh() || !SeePlayer())
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

            if (distX < followRange && SeePlayer())
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

    bool IsPlayerGrounded()
    {
        if (player == null) return false;
        PlayerController pl = player.GetComponent<PlayerController>();
        if (pl == null) return false;
        return pl.IsPlayerGrounded();
    }

    bool SeePlayer()
    {
        Vector2 direction = (player.position.x >= transform.position.x) ? Vector2.right : Vector2.left;
        float distance = Mathf.Abs(player.position.x - transform.position.x);

        if (Physics2D.Raycast(coll.bounds.center, direction, distance, obstacleMask))
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, direction, distance, playerMask);

        return hit.collider != null;
    }

    bool PlayerTooHigh()
    {

        Bounds bear = coll.bounds;

        Collider2D playerColl = player.GetComponent<Collider2D>();
        Bounds playerV = playerColl.bounds;

        float bearTopY = bear.max.y;
        float playerFootY = playerV.min.y;

        float diff = playerFootY - bearTopY;
        float allowed = bear.size.y*5.0f;


        return diff > allowed;
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
        yield return new WaitForSeconds(0.0f);
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
        float x;

        if (isMovingRight)
        {
            x = bounds.max.x;
        }
        else
        {
            x= bounds.min.x;
        }

        Vector2 hitWall = new Vector2(x, bounds.center.y);
        Vector2 dir;

        if (isMovingRight)
        {
            dir = Vector2.right;
        }
        else
        {
            dir = Vector2.left;
        }

        RaycastHit2D hit = Physics2D.Raycast(hitWall, dir, 0.1f, obstacleMask);


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