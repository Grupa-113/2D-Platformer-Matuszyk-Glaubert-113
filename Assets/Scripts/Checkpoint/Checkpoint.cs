using UnityEngine;
using System.Collections; 
using TMPro;

public class Checkpoint : MonoBehaviour
{
    public CanvasGroup dymekGroup;
    public float czasWyswietlania = 2f; 
    public float szybkoscZanikania = 1f; 

    private bool czyAktywowany = false;
    private Animator animator;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !czyAktywowany)
        {
            czyAktywowany = true;
            StartCoroutine(PokazIZaniknij());
        }
    }

    IEnumerator PokazIZaniknij()
    {
        animator = this.GetComponent<Animator>();
        animator.SetBool("smoking", true);

        dymekGroup.alpha = 1f;

        yield return new WaitForSeconds(czasWyswietlania);

 
        while (dymekGroup.alpha > 0)
        {
            dymekGroup.alpha -= Time.deltaTime * szybkoscZanikania;
            yield return null; 
        }

        dymekGroup.alpha = 0;
    }

}