using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public GameObject dialogueBox;
    public GameObject background; 
    public MonoBehaviour playerMovementScript; 

    private Queue<string> sentences;
    private bool isDialogueActive = false;

    void Start()
    {
        sentences = new Queue<string>();
        dialogueBox.SetActive(false);
        background.SetActive(false);
    }

    void Update()
    {
        
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(string[] dialogueLines)
    {
        isDialogueActive = true;
        background.SetActive(true);
        dialogueBox.SetActive(true); 

       
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        sentences.Clear();

        foreach (string sentence in dialogueLines)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            StartCoroutine(EndDialogueAfterDelay());
            return;
        }

        string sentence = sentences.Dequeue();
        textDisplay.text = sentence;
    }

    IEnumerator EndDialogueAfterDelay()
    {
        // Czekamy u³amek sekundy (np. 0.1s), ¿eby klawisz "E" zd¹¿y³ siê "odklikn¹æ"
        yield return new WaitForSeconds(0.1f);

        isDialogueActive = false;
        dialogueBox.SetActive(false);
        background.SetActive(false);

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;
    }
}