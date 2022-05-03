/*
 * Author: James Greensill
 * Date: Unknown
 *
 * Refactor: Calvin Soueid
 * Date: 24/11/2021
 */

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class JournalManager : MonoBehaviour
{
    #region Public Variables

    [Header("Locked Page")]
    [SerializeField]
    private Button seedPacket;

    [SerializeField]
    private TextMeshProUGUI seedPacketNameText;

    [SerializeField]
    private TextMeshProUGUI lockedScientificNameText;

    [SerializeField]
    private TextMeshProUGUI lockedCommonNameText;

    [SerializeField]
    private TextMeshProUGUI lockedFieldNotesText;

    [SerializeField]
    private Image lockedPlantMap;

    [SerializeField]
    private Image lockedPlantImage;

    [Header("Unlocked Page")]
    [SerializeField] public Image BotanicalImage;

    [SerializeField] public Image InGameRepresentationImage;
    [SerializeField] public Image GrowthMap;

    [SerializeField] public TextMeshProUGUI ScientificNameText;
    [SerializeField] public TextMeshProUGUI RegularNameText;
    [SerializeField] public TextMeshProUGUI DescriptionText;
    [SerializeField] public TextMeshProUGUI ScoreText;

    [Header("Other")]
    [SerializeField]
    private GameObject plantTabPrefab;

    [SerializeField]
    private Transform tabLayoutGroup;

    [SerializeField] public GameObject LockedCanvas;
    [SerializeField] public GameObject UnlockedCanvas;

    [SerializeField]
    private GameObject previousButton;

    [SerializeField]
    private GameObject nextButton;

    [SerializeField] public JournalPage StartingPage;

    [SerializeField]
    private JournalPage[] journalPages;

    [SerializeField] public UnityEvent OnNextPageSuccessful;
    [SerializeField] public UnityEvent OnNextPageFailed;
    [SerializeField] public UnityEvent OnPreviousPageSuccessful;
    [SerializeField] public UnityEvent OnPreviousPageFailed;

    #endregion Public Variables

    private JournalPage m_CurrentJournalPage;

    [SerializeField]
    private Journal.SmallJournal smallJournal;

    [SerializeField]
    private PlantInfoNote note;

    public void OpenPage(JournalPage page)
    {
        if (page == null)
            return;

        m_CurrentJournalPage = page;

        LockedPages.Pages.TryGetValue(m_CurrentJournalPage.JournalLink, out var flag);

        if (flag)
        {
            DisplayLockedPage();
        }
        else
        {
            DisplayUnlockedPage();
        }
    }

    private void CreatePlantTabs()
    {
        foreach (JournalPage page in journalPages)
        {
            GameObject tab = Instantiate(plantTabPrefab, tabLayoutGroup);
            tab.GetComponent<Journal.JournalPlantTab>().Initialise(this, page);
        }
    }

    public void Start()
    {
        OpenPage(StartingPage);

        CreatePlantTabs();
    }

    public void Previous()
    {
        DisplayLockedPage();
    }

    public void Next()
    {
        DisplayUnlockedPage();
    }

    // its 11pm. the question marks look nice, don't --- question me.

    /// <summary>
    /// Set the displayed canvas and elements to the locked JournalPage variant.
    /// </summary>
    private void DisplayLockedPage()
    {
        UnlockedCanvas.SetActive(false);
        LockedCanvas.SetActive(true);
        seedPacket.onClick.RemoveAllListeners();
        seedPacket.onClick.AddListener(() =>
        {
            Plant.PlantEnvironment.Instance.StartPlant(m_CurrentJournalPage.Info);
            if (TempSettingsContainer.IsUsingJournal)
            {
                smallJournal.gameObject.SetActive(true);
                note.gameObject.SetActive(false);
                smallJournal.SetPlant(m_CurrentJournalPage);
                smallJournal.SetOpen(true);
            }
            else
            {
                note.gameObject.SetActive(true);
                smallJournal.gameObject.SetActive(false);
                note.SetData(m_CurrentJournalPage, 0);
            }
        });

        // Rebuild the scientific name in "G. species" format
        seedPacketNameText.text = $"{m_CurrentJournalPage.ScientificName.Text[0]}. {m_CurrentJournalPage.ScientificName.Text.Split(' ')[1]}";

        lockedScientificNameText.text = m_CurrentJournalPage.ScientificName.Text;
        lockedCommonNameText.text = m_CurrentJournalPage.CommonName.Text;
        lockedFieldNotesText.text = IO.RichTextHandler.Parse(m_CurrentJournalPage.FieldNotes);
        lockedPlantMap.sprite = m_CurrentJournalPage.GardenMap;
        lockedPlantImage.sprite = m_CurrentJournalPage.ScientificImage;

        previousButton.SetActive(false);
        // Active = true when page is unlocked

        LockedPages.Pages.TryGetValue(m_CurrentJournalPage.JournalLink, out var value);

        nextButton.SetActive(!value);
    }

    /// <summary>
    /// Set the displayed canvas and elements to the unlocked JournalPage variant.
    /// </summary>
    private void DisplayUnlockedPage()
    {
        UnlockedCanvas.SetActive(true);
        LockedCanvas.SetActive(false);

        BotanicalImage.sprite = m_CurrentJournalPage.ScientificImage;
        InGameRepresentationImage.sprite = m_CurrentJournalPage.InGameImage;
        GrowthMap.sprite = m_CurrentJournalPage.GardenMap;
        DescriptionText.text = $"<b>Where: </b>{m_CurrentJournalPage.Where.GetParsed()}\n" +
            $"<b>Size: </b>{m_CurrentJournalPage.Size.GetParsed()}\n" +
            $"<b>Soil: </b>{m_CurrentJournalPage.Soil.GetParsed()}\n" +
            $"<b>Flowering Time: </b>{m_CurrentJournalPage.FloweringTime.GetParsed()}\n" +
            $"<b>Who It Attracts: {m_CurrentJournalPage.WhoItAttracts.GetParsed()}\n" +
            $"<b>Main Pollinator: </b>{m_CurrentJournalPage.MainPollinator.GetParsed()}\n" +
            $"{m_CurrentJournalPage.ExtraInformation.GetParsed()}";
        ScoreText.text = PlayerPrefs.GetInt(m_CurrentJournalPage.ScorePlayerPreferencesName).ToString();
        ScientificNameText.text = m_CurrentJournalPage.ScientificName.Text;

        previousButton.SetActive(true);
        nextButton.SetActive(false);
    }
}