using UnityEngine;

[CreateAssetMenuAttribute(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{

    public string npcName;
    public Sprite npcPortrait;
    public string[] npcDialogueLines;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
    [SerializeField] public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;

}
