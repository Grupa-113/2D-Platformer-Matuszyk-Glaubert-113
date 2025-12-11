using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI nameDisplay;

    public GameObject dialogueBox;

    
    public GameObject player;
    private PlayerController controller;

    private Queue<DialogueLine> sentences;
    private bool isDialogueActive = false;

    void Start()
    {

        controller = player.GetComponent<PlayerController>();

        sentences = new Queue<DialogueLine>();
        dialogueBox.SetActive(false);

    }

    void Update()
    {
        
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(DialogueLine[] dialogueLines)
    {

        

        isDialogueActive = true;
        dialogueBox.SetActive(true); 

        sentences.Clear();

        foreach (DialogueLine sentence in dialogueLines)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

        controller.StopPlayer();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            StartCoroutine(EndDialogueAfterDelay());
            return;
        }

        DialogueLine currentLine = sentences.Dequeue();
        textDisplay.text = currentLine.sentence;
        nameDisplay.text = currentLine.speakerName;
    }

    IEnumerator EndDialogueAfterDelay()
    {

        yield return new WaitForSeconds(0.1f);

        isDialogueActive = false;
        dialogueBox.SetActive(false);


        controller.StartPlayer();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}