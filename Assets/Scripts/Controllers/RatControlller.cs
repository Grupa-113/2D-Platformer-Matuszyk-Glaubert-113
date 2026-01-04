using UnityEngine;

public class RatControlller : EnemyController
{
    [SerializeField] private Transform player;
    private Collider2D coll;
    [SerializeField] private int cherriesToSteal = 10;
    private bool stolen = false;
    private int stolenCherries = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    protected override void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }
    // Update is called once per frame
    protected override void Update()
    {
        if (killed) { return; }

        //porusza sie 
        moveLeft();
        Debug.Log("SZCZUR IDZIE W LEWO");


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
                //col.GetComponent<PlayerController>().Die();
            }
        }
    }
}
