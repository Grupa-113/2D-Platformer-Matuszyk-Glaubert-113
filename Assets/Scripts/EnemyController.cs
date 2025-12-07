using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private bool isFacingRight = true;
    private bool isMovingRight = false;
    [Range(0.01f, 20.0f)][SerializeField] private float moveSpeed = 2.0f;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private float startPositionX;
    private bool killed = false;
    [SerializeField] private float moveRange = 1.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPositionX = this.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (killed) {return;}

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

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }

    void moveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void moveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
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

}
