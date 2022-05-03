using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlantInfoNote : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;

    public void SetData(JournalPage plant, int stageIndex)
    {
        nameText.text = plant.ScientificName.Text;
        descriptionText.text = IO.RichTextHandler.Parse(plant.FieldNotes_Short);
    }
}
