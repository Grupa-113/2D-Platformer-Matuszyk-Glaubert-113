using UnityEngine;

public class GabrysController : MonoBehaviour
{
    private Animator animator;
    public float minWaitTime = 5f;  
    public float maxWaitTime = 15f; 
    private float timer;

    void Start()
    {
        animator = GetComponent<Animator>();
        ResetTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            PlayRandomAnimation();
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = Random.Range(minWaitTime, maxWaitTime);
    }

    void PlayRandomAnimation()
    {
        int choice = Random.Range(0, 2);

        if (choice == 0)
        {
            animator.SetTrigger("Yawn");
        }
        else
        {
            animator.SetTrigger("Stretch");
        }
    }
}
