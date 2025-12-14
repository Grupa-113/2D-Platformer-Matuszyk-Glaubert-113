using UnityEngine;

public class spikes : MonoBehaviour
{

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            player.Die();
        }
    }


    void Update()
    {
        
    }
}
