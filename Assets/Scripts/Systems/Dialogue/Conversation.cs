/*
 *  Author: James Greensill
 *  Date:   2/11/2021
 *  Folder Location: Assets/Scripts/Systems/Dialogue
 */

using UnityEngine;

[System.Serializable]
public class DialogueElement
{

    [Header("This is the audio clip that corresponds to the Dialogue.")]
    [SerializeField]
    public AudioClip DialogueClip;

    [Header("Name of the Dialogue Element.")]
    [SerializeField]
    public string DialogueName;

    [Tooltip("This will sync the length of the dialogue to the clip time of the AudioClip if set to true.")]
    [Header("Sync Dialogue time with AudioClip time.")]
    [SerializeField]
    public bool SyncWithAudio;

    /// <summary>
    /// Characters printed per second when displaying this DialogueElement.
    /// </summary>
    [Min(1)]
    [Tooltip("Characters printed per second when displaying this DialogueElement.")]
    public float CharactersPerSecond = 25;

    [Tooltip("This information will be displayed through the dialogue system.")]
    [Header("Dialogue Text.")]
    [SerializeField]
    [TextArea]
    public string Dialogue;

    public DialogueEmotion Emotion;

    public char[] GetDialogueChars()
    {
        return Dialogue.ToCharArray();
    }

    public int GetDialogueTextLength()
    {
        if (Dialogue != null)
            return Dialogue.Length;
        return 0;
    }
}

[CreateAssetMenu(fileName = "Dialogue Element", menuName = "Scriptable Objects/Dialogue")]
public class Conversation : ScriptableObject
{
    [SerializeField] public Conversation NextConversation;

    [SerializeField]
    public
        DialogueElement[] Dialogue;
}