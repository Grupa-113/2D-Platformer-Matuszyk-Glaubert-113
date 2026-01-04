using System.Collections;
using UnityEngine;

public class RatControlller : EnemyController
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform nest; // teraz: punkt startowy szczura (home)
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;

    private Collider2D coll;
    private SpriteRenderer sprite;

    [SerializeField] private int cherriesToSteal = 10;
    private bool stolen = false;
    private int stolenCherries = 0;

    [SerializeField] private float reachNestDistance = 0.25f;
    [SerializeField] private float cooldownSeconds = 20f;
    [Range(0.01f, 20.0f)][SerializeField] protected float returnSpeed = 2.0f;
    [Range(0.01f, 20.0f)][SerializeField] protected float normalSpeed = 1.0f;
    private enum State { InNest, Chasing, Returning, Cooldown }
    private State state = State.InNest;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    protected override void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();

        state = State.InNest;
        stolen = false;
        stolenCherries = 0;

    }

    private bool ReachedHome()
    {
        return Mathf.Abs(transform.position.x - startPositionX) <= reachNestDistance;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (killed) { return; }

        //porusza sie 
        /*        moveLeft();
                Debug.Log("SZCZUR IDZIE W LEWO");*/

        switch (state)
        {
            case State.InNest:
                // stoi w miejscu i czeka a¿ zobaczy gracza
                if (SeePlayer())
                {
                    state = State.Chasing;
                }
                break;

            case State.Chasing:
                MoveToX(player.position.x);
                break;

            case State.Returning:
                MoveToX(startPositionX);
                SetTargetSpeed(returnSpeed);

                if (ReachedHome())
                {
                    StartCoroutine(CooldownRoutine());
                    SetTargetSpeed(normalSpeed);
                }
                break;

            case State.Cooldown:
                // nic
                break;
        }
    }

    private IEnumerator CooldownRoutine()
    {
        state = State.Cooldown;
        yield return new WaitForSeconds(cooldownSeconds);

        // po 20s: jeœli nadal widzi gracza -> wychodzi od razu
        if (SeePlayer())
        {

            state = State.Chasing;
        }
        else
        {
            state = State.InNest;
        }
    }

    protected override void moveLeft()
    {
        Debug.Log("SZCZUR");
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (killed) { return; };

            float playerBottom = col.bounds.min.y;
            float enemyTop = GetComponent<Collider2D>().bounds.max.y;

            if (playerBottom > enemyTop - heightMarginToDead)
            {
                PlayerController player = col.GetComponent<PlayerController>();
                player.BounceFromEnemy();

                if(stolen && stolenCherries > 0)
                {
                    int returnCherries = Mathf.Max(0, stolenCherries / 2);
                    GameManager.instance.AddPoints(returnCherries);
                }

                Die();
            }
            else
            {
                stolen = true;
                stolenCherries += cherriesToSteal;

                GameManager.instance.AddPoints(-cherriesToSteal);
                state = State.Returning;

                //col.GetComponent<PlayerController>().Die();
            }
        }
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
            if (!isFacingRight) Flip();  // z EnemyController
            moveRight();                  // z EnemyController
        }
        else
        {
            if (isFacingRight) Flip();   // z EnemyController
            moveLeft();                   // z EnemyController
        }
    }
    private void SetTargetSpeed(float target)
    {
        moveSpeed = target;
    }

}
