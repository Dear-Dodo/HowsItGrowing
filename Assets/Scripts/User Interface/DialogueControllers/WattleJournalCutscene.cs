/*
 *  Author: James Greensill
 *  Folder Location: Assets/Scripts/User Interface/DialogueControllers/
 */

using UnityEngine;
using UnityEngine.UI;

public class WattleJournalCutscene : MonoBehaviour
{
    [SerializeField] public Conversation startConversation;
    [SerializeField] public Conversation endConversation;

    [SerializeField] private Conversation currentConversation;
    [SerializeField] private Selectable[] objectsToDisable;

    private void Awake()
    {
        DialogueSystem.Instance.onCurrentConversationEnd?.RemoveAllListeners();
        DialogueSystem.Instance.onDialogueClose?.RemoveAllListeners();
    }

    /// <summary>
    /// Verifies what dialogue should be played depending on how many pages are locked.
    /// </summary>
    private void Start()
    {
        if (LockedPages.Verify())
        {
            Play(endConversation);
        }
        else Play(startConversation);
    }

    /// <summary>
    /// Plays a conversation with the Journal behind the DialogueSystem (and disables all objects from interaction).
    /// </summary>
    /// <param name="conversationToPlay"></param>
    private void Play(Conversation conversationToPlay)
    {
        currentConversation = conversationToPlay;

        IsSelectable(false);
        UserInterfaceStack.Instance.HideAllMenus();

        DialogueSystem.Instance.Play(currentConversation, false);
        UserInterfaceStack.Instance.AddLayer("JournalUI");

        DialogueSystem.Instance.onCurrentConversationEnd.AddListener(() =>
        {
            IsSelectable(true);
        });
    }

    /// <summary>
    /// Activates/Deactivates list of objects depending on the flag.
    /// </summary>
    /// <param name="flag"></param>
    private void IsSelectable(bool flag)
    {
        foreach (var selectable in objectsToDisable)
        {
            selectable.interactable = flag;
        }
    }
}