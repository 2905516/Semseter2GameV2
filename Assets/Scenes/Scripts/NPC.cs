using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue Data")]
    public NPCDialogue dialogueData;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping;
    private bool isDialogueActive;

    private void Update()
    {
        // Close dialogue with Escape if active
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Escape))
            EndDialogue();
    }

    // IInteractable: can player start an interaction?
    // Use Time.timeScale to determine pause instead of referencing PauseMenu.isPaused statically.
    public bool CanInteract()
    {
        // You can only start interacting if no dialogue is currently active and the game isn't paused.
        return !isDialogueActive && Time.timeScale != 0f;
    }

    public void Interact()
    {
        // Prevent interaction while paused
        if (Time.timeScale == 0f)
            return;

        // Unlock cursor for dialogue
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        // Validate data
        if (dialogueData == null || dialogueData.npcDialogueLines == null || dialogueData.npcDialogueLines.Length == 0)
            return;

        // Progress or start dialogue
        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        // Validate UI references
        if (nameText != null) nameText.SetText(dialogueData.npcName);
        if (portraitImage != null) portraitImage.sprite = dialogueData.npcPortrait;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (dialogueText != null)
            dialogueText.SetText("");

        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        if (isTyping)
        {
            // Finish current line instantly
            StopAllCoroutines();
            if (dialogueText != null)
                dialogueText.SetText(dialogueData.npcDialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.npcDialogueLines.Length)
        {
            // Continue to next line
            StartCoroutine(TypeLine());
        }
        else
        {
            // End dialogue sequence
            EndDialogue();
        }
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        if (dialogueText != null)
            dialogueText.SetText("");

        string line = dialogueData.npcDialogueLines[dialogueIndex];

        // Use realtime waits so typing continues while Time.timeScale == 0 (dialogue UI should run in real time)
        foreach (char letter in line)
        {
            if (dialogueText != null)
                dialogueText.text += letter;

            // Optional: play voice or type sound here
            // SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);

            yield return new WaitForSecondsRealtime(Mathf.Max(0.001f, dialogueData.typingSpeed));
        }

        isTyping = false;

        // Auto-advance lines if enabled (safe bounds checks)
        if (dialogueData.autoProgressLines != null &&
            dialogueData.autoProgressLines.Length > dialogueIndex &&
            dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSecondsRealtime(Mathf.Max(0f, dialogueData.autoProgressDelay));
            NextLine();
        }
    }

    public void EndDialogue()
    {
        SoundEffectManager.Play("Button");

        StopAllCoroutines();
        isDialogueActive = false;

        if (dialogueText != null)
            dialogueText.SetText("");
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        // Lock cursor again ONLY if the game is not paused (Time.timeScale)
        if (Time.timeScale != 0f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
