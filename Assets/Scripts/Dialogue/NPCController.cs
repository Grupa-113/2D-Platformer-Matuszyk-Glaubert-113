using UnityEngine;

public class NPCController : MonoBehaviour
{
    
    public DialogueLine[] mainDialogue;
    [SerializeField] private bool useEndDialogue;
    public DialogueLine[] endDialogue;

    private bool isPlayerClose = false;
    private bool possibleTalk = false;
    private bool whiletalk = false;
    private bool talkInPast = false; 

    private DialogueManager dialogueManager;
    
    public GameObject button;
    public GameObject player;

    public bool playerFacingRight;

    void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        button.SetActive(false);
    }

    void Update()
    {
        possibleTalk =(playerFacingRight == player.GetComponent<PlayerController>().GetFacingRight());
        whiletalk = dialogueManager.GetComponent<DialogueManager>().IsDialogueActive();

        if (isPlayerClose && Input.GetKeyDown(KeyCode.E) && possibleTalk)
        {
            if (!dialogueManager.dialogueBox.activeInHierarchy)
            {
                DialogueLine[] dialogue;

                dialogue = (talkInPast) ? endDialogue : mainDialogue;
                talkInPast = (useEndDialogue) ? true : false ;

                dialogueManager.StartDialogue(dialogue, transform);
            }
        }
        if(possibleTalk && isPlayerClose && !whiletalk)
        {
            button.SetActive(true);
        }
        else
        {
            button.SetActive(false);
        }
    }

    // Wykrywanie wejœcia w strefê NPC
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerClose = true;
            
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

//klasa do dialogów 
[System.Serializable] 
public class DialogueLine
{
    public string speakerName; 
    [TextArea(3, 10)]
    public string sentence;    
}