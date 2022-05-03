/*
 *  Author: James Greensill
 *  Date:   2/11/2021
 *  Folder Location: Assets/Scripts/Systems/Dialogue
 *
 *  Refactor: Calvin Soueid
 *  Date: 22/11/2021
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityAsync;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;

/// <summary>
/// This is the dialogue emotion that is used to link an emotion to a sprite; avoided using string comparisons at cost of modularity.
/// </summary>
public enum DialogueEmotion
{
    Happy,
    Thinking,
    Confused,
    Disagree,
    Static,
    Glow
}

[RequireComponent(typeof(AudioSource))]
public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

#pragma warning disable 0649

    /// <summary>
    /// Audio clip played each time a character is written
    /// </summary>
    [Tooltip("Audio clip played each time a character is written")]
    [SerializeField]
    private AudioClip characterWriteAudio;

    [SerializeField]
    private GameObject dialogueBackground;

    /// <summary>
    /// Unity serializable form of a dictionary. Converted to a dictionary at runtime.
    /// </summary>
    [Tooltip("Unity serializable form of a dictionary. Converted to a dictionary at runtime.")]
    [SerializeField]
    private DialogueEmotionPair[] dialogueEmotionArray = new DialogueEmotionPair[Enum.GetValues(typeof(DialogueEmotion)).Length];

    /// <summary>
    /// Associates DialogueEmotions with sprites. Built at runtime.
    /// </summary>
    private Dictionary<DialogueEmotion, Sprite> dialogueEmotions = new Dictionary<DialogueEmotion, Sprite>();

    /// <summary>
    /// Image representing the emotive narrator.
    /// </summary>
    [Tooltip("Image representing the emotive narrator.")]
    [SerializeField]
    private Image narrator;

    private Animator narratorAnimator;
    private Button narratorButton;

    /// <summary>
    /// Component to display dialogue text
    /// </summary>
    [Tooltip("Component to display dialogue text")]
    [SerializeField]
    private TextMeshProUGUI dialogueTextComponent;

    private Conversation currentConversation;
    private int? currentDialogueIndex;
    private CancellationTokenSource cancellationSource = new CancellationTokenSource();

    /// <summary>
    /// Currently running dialogue printing task.
    /// </summary>
    private Task currentPrintTask;

    public UnityEvent onCurrentConversationEnd;

    public UnityEvent onDialogueClose;

    private bool IsPrintRunning()
    {
        return currentPrintTask != null && !currentPrintTask.IsCompleted;
    }

    private bool dialogueSkipTrigger = false;

    /// <summary>
    /// AudioSource for the dialogue system
    /// </summary>
    private AudioSource dialogueSource;

    private void Awake()
    {
        if (Instance != null)
        {
            throw new Exception("An instance of DialogueSystem already exists");
        }
        Instance = this;

        dialogueSource = GetComponent<AudioSource>();
        dialogueSource.clip = characterWriteAudio;

        narratorAnimator = narrator.GetComponent<Animator>();
        narratorButton = narrator.GetComponent<Button>();

        foreach (DialogueEmotionPair pair in dialogueEmotionArray)
        {
            dialogueEmotions.Add(pair.Emotion, pair.EmotionSprite);
        }
    }

    /// <summary>
    /// Begin a new dialogue conversation
    /// </summary>
    /// <param name="conversation"></param>
    public void Play(Conversation conversation, bool closeMenu = true, bool openDialogue = true)
    {
        if (currentConversation != null)
        {
            Stop(false);
        }
        else if (openDialogue)
        {
            UserInterfaceStack.Instance.Show("DialogueUI", closeMenu);
            narratorAnimator.SetTrigger("WattleSlideIn");
        }

        dialogueBackground.SetActive(true);
        currentConversation = conversation;
        currentDialogueIndex = 0;

        Continue();
    }

    /// <summary>
    /// Cause wattle to pop up and become clickable to start the given conversation.
    /// </summary>
    /// <param name="conversation"></param>
    public void SuggestStartDialogue(Conversation conversation)
    {
        dialogueBackground.SetActive(false);
        UserInterfaceStack.Instance.Show("DialogueUI", true);
        narratorButton.onClick.RemoveAllListeners();
        narrator.sprite = dialogueEmotions[conversation.Dialogue[0].Emotion];
        narratorAnimator.SetTrigger("WattleSlideIn");
        narratorButton.interactable = true;
        narratorButton.onClick.AddListener(() =>
        {
            Play(conversation, false);
            narratorButton.interactable = false;
            narratorButton.onClick.RemoveAllListeners();
        });
    }

    /// <summary>
    /// Skip to the end of the current dialogue printing or move to the next.
    /// </summary>
    public void Continue()
    {
        if (IsPrintRunning())
        {
            dialogueSkipTrigger = true;
        }
        else if (currentDialogueIndex != null)
        {
            if (currentDialogueIndex >= currentConversation.Dialogue.Length)
            {
                Stop(true);
                return;
            }
            currentPrintTask = PrintDialogue(currentConversation.Dialogue[(int)currentDialogueIndex], cancellationSource.Token);
        }
    }

    /// <summary>
    /// Start the current conversation from the beginning.
    /// </summary>
    public void Restart()
    {
        Conversation old = currentConversation;
        if (IsPrintRunning())
        {
            Stop(false, false, false);
            currentConversation = old;
        }
        Play(old);
    }

    /// <summary>
    /// Stop the current printing operation if one exists and hide the dialogue box.
    /// </summary>
    /// <param name="hideUI"></param>
    public async void Stop(bool hideUI, bool callEvents = true, bool clearEvents = true)
    {
        currentConversation = null;
        currentDialogueIndex = null;
        cancellationSource.Cancel();
        cancellationSource = new CancellationTokenSource();
        dialogueSource.Stop();
        if (callEvents)
        {
            onCurrentConversationEnd?.Invoke();
        }
        if (clearEvents)
        {
            onCurrentConversationEnd.RemoveAllListeners();
        }

        if (hideUI)
        {
            narratorAnimator.SetTrigger("WattleSlideOut");
            await Await.Seconds(1);
            UserInterfaceStack.Instance.Hide("DialogueUI");
            onDialogueClose?.Invoke();
            onDialogueClose?.RemoveAllListeners();
        }
    }

    /// <summary>
    /// Print the contents of the provided dialogue to the dialogue box at a determined rate.
    /// </summary>
    /// <param name="dialogue"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task PrintDialogue(DialogueElement dialogue, CancellationToken token)
    {
        try
        {
            if (dialogueEmotions.TryGetValue(dialogue.Emotion, out Sprite emote))
            {
                narrator.sprite = emote;
            }
            else
            {
                Debug.LogWarning($"Emotion {dialogue.Emotion} does not exist in dictionary");
            }
            dialogueTextComponent.text = "";

            int currentIndex = 0;
            float accumulatedTime = 0;
            float interval = 1f / dialogue.CharactersPerSecond;

            string parsedDialogue = IO.RichTextHandler.Parse(dialogue.Dialogue);

            while (currentIndex < parsedDialogue.Length)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                if (dialogueSkipTrigger)
                {
                    dialogueSkipTrigger = false;
                    dialogueTextComponent.text = parsedDialogue;
                    break;
                }

                int charactersToPrint = (int)(accumulatedTime / interval);

                accumulatedTime %= interval;

                if (charactersToPrint > 0 && characterWriteAudio != null)
                {
                    dialogueSource.Play();
                }

                for (int i = 0; i < charactersToPrint; i++)
                {
                    if (currentIndex >= parsedDialogue.Length)
                    {
                        break;
                    }
                    char next = parsedDialogue[currentIndex];

                    if (next == '<')
                    {
                        dialogueTextComponent.text += IO.RichTextHandler.ParseUpcomingTag(parsedDialogue, currentIndex, out currentIndex);
                        continue;
                    }

                    dialogueTextComponent.text += parsedDialogue[currentIndex];
                    currentIndex++;
                }

                accumulatedTime += Time.deltaTime;
                await Await.NextUpdate();
            }
            currentDialogueIndex++;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }
}