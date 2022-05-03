/*
 * Author: Calvin Soueid
 * Date: 23/11/2021
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Journal
{
    public class JournalPlantTab : MonoBehaviour
    {
        private JournalPage targetPlant;
        private JournalManager journal;

        [SerializeField]
        private Image checkbox;
        [SerializeField]
        private TextMeshProUGUI plantNameText;

        [SerializeField]
        private Sprite unlockedSprite;
        [SerializeField]
        private Sprite lockedSprite;


        public void Initialise(JournalManager journalManager, JournalPage target)
        {
            targetPlant = target;
            journal = journalManager;

            plantNameText.text = targetPlant.ScientificName.Text.Split(' ')[0];
            checkbox.sprite = LockedPages.Pages[target.JournalLink] ? lockedSprite : unlockedSprite;

            GetComponent<Button>().onClick.AddListener(() => journal.OpenPage(target));
            GetComponent<Image>().color = target.JournalTabColour;
        }
    } 
}
