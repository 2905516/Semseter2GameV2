using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;


    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        //if no data or the game is paused and no dialogue is active
        if (dialogueData == null || (PauseMenu.isPaused = true && isDialogueActive))
        {

            return;

        }

        if (isDialogueActive)
        {
            NextLine();

        }
        else
        {

            StartDialogue();


        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);


        StartCoroutine(TypeLine());


    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.npcDialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.npcDialogueLines.Length)
        {
            StartCoroutine(TypeLine());

        }
        else
        {
            EndDialogue();


        }


    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.npcDialogueLines[dialogueIndex])
        {

            dialogueText.text += letter;
            //SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);
            yield return new WaitForSeconds(dialogueData.typingSpeed);


        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();

        }

    }

    public void EndDialogue()
    {
        SoundEffectManager.Play("Button");
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);




    }

}
