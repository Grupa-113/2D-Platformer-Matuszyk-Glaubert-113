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
    
    public GameObject player;
    private PlayerController controller;

    private Queue<string> sentences;
    private bool isDialogueActive = false;

    void Start()
    {

        controller = player.GetComponent<PlayerController>();

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

        sentences.Clear();

        foreach (string sentence in dialogueLines)
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

        string sentence = sentences.Dequeue();
        textDisplay.text = sentence;
    }

    IEnumerator EndDialogueAfterDelay()
    {

        yield return new WaitForSeconds(0.1f);

        isDialogueActive = false;
        dialogueBox.SetActive(false);
        background.SetActive(false);

        controller.StartPlayer();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}