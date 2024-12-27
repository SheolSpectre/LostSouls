using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class DialogueManager : MonoBehaviour {
    private bool isDialogueActive = false;
    public Text dialogueText; // Drag your UI Text here
    public GameObject dialogueBox; // Drag your dialogue panel here
    private Queue<string> lines; // Store dialogue lines
    [SerializeField] private Player scriptPlayer;
    void Start() {
        lines = new Queue<string>();
    }
    void Update() {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space)) {
            DisplayNextLine();
        }
    }
    public void StartDialogue(Dialogue dialogue) {
        dialogueBox.SetActive(true);
        isDialogueActive = true;
        if (scriptPlayer != false) {
            scriptPlayer.enabled = false;
        }
        lines.Clear();
        foreach (string line in dialogue.lines) {
            lines.Enqueue(line);
        }
        DisplayNextLine();
    }
    public void EndDialogue() {
        dialogueBox.SetActive(false);
        isDialogueActive = false;
        if (scriptPlayer != null) {
            scriptPlayer.enabled = true;
        }
        FindAnyObjectByType<Human>()?.DisappearAfterDialogue();
    }
    public void DisplayNextLine() {
        if (lines.Count == 0) {
            EndDialogue();
            return;
        }
        string currentLine = lines.Dequeue();
        dialogueText.text = currentLine;
    }
}
