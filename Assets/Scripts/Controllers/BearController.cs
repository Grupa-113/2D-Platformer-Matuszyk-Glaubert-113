using UnityEngine;
using System.Collections;

public class BearEnemy : EnemyController
{

    [SerializeField] private float followSpeed = 6.0f;
    [SerializeField] private float normalSpeed = 2.0f;
    [SerializeField] private float followRange = 8.0f;
    [SerializeField] private Transform player;

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;

    private Collider2D coll;
    private bool isFollowing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    protected override void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isMovingRight = transform.localScale.x > 0;
        isFacingRight = isMovingRight;
    }

    // Update is called once per frame
    protected override void Update()
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

    protected override void moveRight()
    {
        rigidBody.linearVelocity = new Vector2(moveSpeed, rigidBody.linearVelocity.y);
        // transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    protected override void moveLeft()
    {
        rigidBody.linearVelocity = new Vector2(-moveSpeed, rigidBody.linearVelocity.y);
        //transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
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
        float allowed = bear.size.y * 5.0f;


        return diff > allowed;
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
        Vector2 direction;

        if (isMovingRight)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }

        float x;

        if (isMovingRight)
        {
            x = bounds.max.x;
        }
        else
        {
            x = bounds.min.x;
        }

        Vector2 hitWall = new Vector2(x, bounds.center.y);
        RaycastHit2D hit = Physics2D.Raycast(hitWall, direction, 0.1f, obstacleMask);
        Debug.DrawRay(hitWall, direction * 0.1f, Color.red);

        if(hit.collider == null)
        {
            return false;
        }

        return hit.collider.CompareTag("Platform") ;
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

    //protected override void Die()
    //{
    //    base.Die();
    //}
}