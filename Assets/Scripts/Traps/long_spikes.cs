using System.Collections;
using UnityEngine;

public class long_spikes : MonoBehaviour
{
    [Header("Time Settings")] 
    [SerializeField] private float timeDown = 2.0f;   
    [SerializeField] private float timeUp = 2.0f;       

    [Header("Animation Settings")]
    [SerializeField] private float UPDuration = 0.4f; 
    [SerializeField] private float DOWNDuration = 0.4f; 

    [Header("Particles")]
    [SerializeField] private GameObject drillEfect;
    [SerializeField] private float offsetdrillEfect = 0.1f;


    private Animator anim;
    private BoxCollider2D damageArea; 

    void Start()
    {
        anim = GetComponent<Animator>();
        damageArea = GetComponent<BoxCollider2D>();
        damageArea.enabled = false;

        StartCoroutine(TrapRoutine());
    }

    IEnumerator TrapRoutine()
    {
        while (true) 
        {


            yield return new WaitForSeconds(timeDown);
            anim.SetBool("DoUp", true); 
            Instantiate(drillEfect, new Vector3(transform.position.x, transform.position.y + offsetdrillEfect, transform.position.z), Quaternion.identity);



            yield return new WaitForSeconds(UPDuration * 0.5f);
            damageArea.enabled = true; 
            yield return new WaitForSeconds(UPDuration * 0.5f);



            yield return new WaitForSeconds(timeUp);
            anim.SetBool("DoUp", false); 


            yield return new WaitForSeconds(DOWNDuration * 0.5f);
            damageArea.enabled = false; 
            yield return new WaitForSeconds(DOWNDuration * 0.5f);

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
