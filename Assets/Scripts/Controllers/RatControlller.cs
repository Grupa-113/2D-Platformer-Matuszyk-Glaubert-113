using System.Collections;
using UnityEngine;


public class RatControlller : EnemyController
{
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;

   
    [SerializeField] private int cherriesToSteal = 10;
    private bool stolen = false;
    private int stolenCherries = 0;

    [SerializeField] private float reachNestDistance = 0.001f;
    [SerializeField] private float cooldownSeconds = 20f;

    private enum State { InNest, Chasing, Returning, Cooldown }
    private State state = State.InNest;

    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip stealcoinSound;

    private AudioSource source;
    private Collider2D coll;
    private SpriteRenderer sprite;

    protected override void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();

        state = State.InNest;
        stolen = false;
        stolenCherries = 0;

    }

    protected override void Update()
    {
        if (killed) { 
            return; 
        }

        switch (state)
        {
            case State.InNest:
                animator.SetBool("isRunning", false);

                if (SeePlayer())
                {
                    state = State.Chasing;
                }
                break;

            case State.Chasing:
                animator.SetBool("isRunning", true);
                MoveToX(player.position.x);
                break;

            case State.Returning:
                MoveToX(startPositionX);
                animator.SetBool("isRunning", true);

                if (ReachedHome())
                {
                    StartCoroutine(CooldownRoutine());
                    animator.SetBool("isRunning", false);
                    Flip();
                }
                break;

            case State.Cooldown:
                animator.SetBool("isRunning", false);
                break;
        }
    }

    private bool ReachedHome()
    {
        return Mathf.Abs(transform.position.x - startPositionX) < reachNestDistance;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (killed) { 
                return; 
            };

            float playerBottom = col.bounds.min.y;
            float enemyTop = GetComponent<Collider2D>().bounds.max.y;

            if (playerBottom > enemyTop - heightMarginToDead)
            {
                PlayerController player = col.GetComponent<PlayerController>();
                player.BounceFromEnemy();

                if (stolen && stolenCherries > 0)
                {
                    int returnCherries = Mathf.Max(0, stolenCherries / 2);
                    AudioSource.PlayClipAtPoint(coinSound, transform.position, 1f);
                    GameManager.instance.AddPoints(returnCherries);
                }
                Die();
            }else{
                stolen = true;
                stolenCherries += cherriesToSteal;

                GameManager.instance.AddPoints(-cherriesToSteal);
                source.PlayOneShot(stealcoinSound, AudioListener.volume);

                state = State.Returning;

            }
        }
    }

    private IEnumerator CooldownRoutine()
    {
        state = State.Cooldown;
        yield return new WaitForSeconds(cooldownSeconds);

        if (SeePlayer()){
            state = State.Chasing;
        }else{ 
            state = State.InNest;
        }
    }

    protected override void moveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
    }

    private bool SeePlayer()
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

    private void MoveToX(float targetX)
    {
        if (targetX > transform.position.x)
        {

            if (!isFacingRight)
            {
                Flip();
            }
            moveRight();
        }
        else
        {
            if (isFacingRight)
            {
                Flip(); 
            }
            moveLeft();
        }
    }


    private void FaceRight()
    {
        if (!isFacingRight)
        {
            Flip();
        }
                    
    }

} 
