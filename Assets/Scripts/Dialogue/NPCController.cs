using UnityEngine;

public class NPCController : MonoBehaviour
{
    [TextArea(3, 10)] 
    public string[] dialogueLines; 

    private bool isPlayerClose = false;
    private DialogueManager dialogueManager;
    public GameObject button;  


    void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        button.SetActive(false);
    }

    void Update()
    {
        if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueManager.dialogueBox.activeInHierarchy)
            {
                dialogueManager.StartDialogue(dialogueLines);
            }
        }
    }

    // Wykrywanie wejœcia w strefê NPC
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerClose = true;
            button.SetActive(true);
        }
    }

    // Wykrywanie wyjœcia ze strefy NPC
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerClose = false;
            button.SetActive(false);

        }
    }
}