using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {
    private bool isDialogueActive = false;
    public Text dialogueText; // Drag your UI Text here
    public GameObject dialogueBox; // Drag your dialogue panel here
    private Queue<string> lines; // Store dialogue lines
    [SerializeField] private Player scriptPlayer; // Reference to disable player controls
    public AudioSource audioSource;
    public Dialogue currentDialogue;

    void Start() {
        lines = new Queue<string>();
    }

    void Update() {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space)) {
            DisplayNextLine();
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        currentDialogue = dialogue;
        dialogueBox.SetActive(true);
        isDialogueActive = true;

        if (scriptPlayer != null) {
            scriptPlayer.enabled = false; // Disable player controls during dialogue
        }

        lines.Clear();

        foreach (string line in currentDialogue.lines) {
            lines.Enqueue(line);
        }

        // Validate voiceovers
        if (currentDialogue.voiceovers == null || currentDialogue.voiceovers.Length != currentDialogue.lines.Length) {
            Debug.LogWarning("Voiceovers array is null or does not match lines array in length.");
        }

        DisplayNextLine();
    }

    public void DisplayNextLine() {
        if (lines.Count == 0) {
            EndDialogue();
            return;
        }

        string currentLine = lines.Dequeue();
        dialogueText.text = currentLine;

        // Play corresponding voiceover
        if (currentDialogue.voiceovers != null && lines.Count < currentDialogue.voiceovers.Length) {
            int clipIndex = currentDialogue.voiceovers.Length - lines.Count - 1; // Get the matching clip index
            AudioClip clip = currentDialogue.voiceovers[clipIndex];
            if (clip != null) {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    public void EndDialogue() {
        dialogueBox.SetActive(false);
        isDialogueActive = false;

        if (scriptPlayer != null) {
            scriptPlayer.enabled = true; // Re-enable player controls
        }

        FindAnyObjectByType<Human>()?.DisappearAfterDialogue();
    }
}
