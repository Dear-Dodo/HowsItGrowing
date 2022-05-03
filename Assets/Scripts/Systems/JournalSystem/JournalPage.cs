using UnityEngine;

[CreateAssetMenu(fileName = "Journal Page", menuName = "Scriptable Objects/Journal")]
public class JournalPage : ScriptableObject
{
    [Tooltip("Plant game data")]
    public Plant.Data.PlantInfo Info;

    [Header("The official scientific name of this plant.")]
    [SerializeField] public RichString ScientificName;

    [Header("The good' ol common casual name for this plant.")]
    [SerializeField] public RichString CommonName;

    /// <summary>
    /// Displayed on the locked version of the journal entry, intended to give players an idea of what variables a plant would prefer.
    /// </summary>
    [Tooltip("Displayed on the locked version of the journal entry, intended to give players an idea of what variables a plant would prefer.")]
    [TextArea(5,5)]
    public string FieldNotes;

    [Tooltip("")]
    [TextArea(5, 5)]
    public string FieldNotes_Short;

    [Header("This Information will be put into the Description box")]
    [SerializeField] public RichString Where;

    [SerializeField] public RichString Size;
    [SerializeField] public RichString Soil;
    [SerializeField] public RichString FloweringTime;
    [SerializeField] public RichString WhoItAttracts;
    [SerializeField] public RichString MainPollinator;
    [SerializeField] public RichString ExtraInformation;

    [Header("The Scientific Image of this plant.")]
    [SerializeField] public Sprite ScientificImage;

    [Header("The in-game representation of this plant.")]
    [SerializeField] public Sprite InGameImage;

    [Header("The botanical gardens location map.")]
    [SerializeField] public Sprite GardenMap;

    /// <summary>
    /// The colour of the journal 'post-it note' tab for this plant.
    /// </summary>
    [Tooltip("The colour of the journal 'post-it note' tab for this plant.")]
    public Color JournalTabColour = Color.red;

    [Header("The players current score.")]
    [SerializeField] public int Score;

    [Header("RanunculusScore, WaratahScore, BanksiaScore")]
    [SerializeField] public string ScorePlayerPreferencesName;

    [Header("Joournal Link")] [SerializeField]
    public string JournalLink;
}