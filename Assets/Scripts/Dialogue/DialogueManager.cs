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


    [Header("Ustawienia Pisania")]
    public float typingSpeed = 0.05f; 
    private bool isTyping = false; 
    private string currentSentence = "";


    [Header("kamera zoom")]
    public UnityStandardAssets._2D.CameraFollow cameraScript;


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
            if (isTyping) //anulowanie efektu pisania 
            {
                StopAllCoroutines();
                textDisplay.text = currentSentence;
                isTyping = false;
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(DialogueLine[] dialogueLines, Transform npcTransform)
    {

        cameraScript.SetZoom(true, npcTransform);

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
        nameDisplay.text = currentLine.speakerName;


        textDisplay.text = currentLine.sentence;

        currentSentence = currentLine.sentence;


        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));


    }

    IEnumerator EndDialogueAfterDelay()
    {
        cameraScript.SetZoom(false);
        yield return new WaitForSeconds(0.1f);

        isDialogueActive = false;
        dialogueBox.SetActive(false);


        controller.StartPlayer();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
    

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        textDisplay.text = ""; 

        foreach (char letter in sentence.ToCharArray())
        {
            textDisplay.text += letter; 
            yield return new WaitForSeconds(typingSpeed); 
        }

        isTyping = false; 
    }
}