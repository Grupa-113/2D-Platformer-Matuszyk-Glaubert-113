using System.Collections;
using System.Xml.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected bool isFacingRight = true;
    protected bool isMovingRight = false;

    [Range(0.01f, 20.0f)][SerializeField] protected float moveSpeed = 2.0f;
    [SerializeField] protected float moveRange = 1.0f;

    protected Animator animator;
    protected Rigidbody2D rigidBody;
    protected float startPositionX;
    protected bool killed = false;

    [SerializeField] protected GameObject deathEffect;
    [SerializeField] protected Collider2D head;

    [SerializeField] private float heightMarginToDead = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    protected virtual void Awake()
    {

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPositionX = this.transform.position.x;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (killed) { return; }

        if (isMovingRight)
        {
            if (this.transform.position.x < startPositionX + moveRange)
            {
                //porusza sie 
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
                //porusza sie 
                moveLeft();
            }
            else
            {
                isMovingRight = true;
                Flip();
            }
        }
    }

    protected void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }

    protected virtual void moveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    protected virtual void moveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }
    protected IEnumerator KillOnAnimationEnd()
    {
        /* yield return new WaitForSecond(time_in_seconds);*/
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (killed) { return; }
            ;


            float playerBottom = col.bounds.min.y;
            float enemyTop = GetComponent<Collider2D>().bounds.max.y;

            if (playerBottom > enemyTop - heightMarginToDead)
            {
                Debug.Log("skakanko");
                PlayerController player = col.GetComponent<PlayerController>();
                player.BounceFromEnemy();
                Die();
            }
            else
            {
                col.GetComponent<PlayerController>().Die();
            }
        }
    }
    protected virtual void Die()
    {

        killed = true;
        GetComponent<Collider2D>().enabled = false;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        //animator.SetBool("isDead", true); 
        StartCoroutine(KillOnAnimationEnd());
        GameManager.instance.AddenemiesKill(1);
    }

    private void OnDrawGizmos()
    {
        float width = GetComponent<Collider2D>().bounds.size.x;
        float topY = GetComponent<Collider2D>().bounds.max.y - heightMarginToDead;

        Vector3 startPoint = new Vector3(transform.position.x - (width / 2), topY, transform.position.z);

        Debug.DrawRay(startPoint, Vector3.right * width, Color.red);

    }
}