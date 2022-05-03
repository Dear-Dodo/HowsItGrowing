/*
 *  Author: Calvin Soueid
 *  Date:   15/11/2021
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

namespace Plant.Data
{
    /// <summary>
    /// Structure storing data for a single condition check failure.
    /// "LockedVariables" should contain the types of the enums that should be locked
    /// For example: typeof(Plant.WaterLevel) is a valid item
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "Data/HelpStep", order = 3)]
    public class HelpStep : ScriptableObject
    {
        /// <summary>
        /// Whether or not to use the dialogue override for the fail state associated with this help step.
        /// Note: You can override on the PlantStageInfo for a stage-wide custom fail dialogue.
        /// </summary>
        [Tooltip("Whether or not to use the dialogue override for the fail state associated with this help step. Note: You can override on the PlantStageInfo for a stage-wide custom fail dialogue.")]
        public bool IsFailDialogueOverridden = false;
        public DialogueElement FailDialogue;

        [Space(10)]
        public DialogueElement[] HintDialogue;
        public List<SerialisedType> LockedVariables = new List<SerialisedType>();
    }

}