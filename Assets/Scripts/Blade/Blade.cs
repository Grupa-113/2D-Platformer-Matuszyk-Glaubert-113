using UnityEngine;

public class Blade : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {

            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }

        }
    }
}
