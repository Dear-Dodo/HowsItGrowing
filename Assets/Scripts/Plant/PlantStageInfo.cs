/*
 *  Author: Calvin Soueid
 *  Date:   15/11/2021
 */

using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plant.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "Data", menuName = "Data/PlantStage", order = 2)]
    public class PlantStageInfo : ScriptableObject
    {
        public CameraPosition CameraPos;
        public bool IsCheckConditionsVisible = true;
        public StageRequirement Requirements;
        public GameObject PlantPrefab;

        public bool IsFailDialogueOverridden = false;
        public DialogueElement FailDialogue;

        [Space(10)]
        public HelpStep[] HelpSteps;

        [HideInInspector]
        public PlantStage Stage;
        [HideInInspector]
        public int FailCount;


        public static PlantStageInfo Clone(PlantStageInfo from)
        {
            PlantStageInfo result = (PlantStageInfo)ScriptableObject.CreateInstance(typeof(PlantStageInfo));
            result.CameraPos = from.CameraPos;
            result.IsCheckConditionsVisible = from.IsCheckConditionsVisible;
            result.Requirements = from.Requirements;
            result.PlantPrefab = from.PlantPrefab;
            result.IsFailDialogueOverridden = from.IsFailDialogueOverridden;
            result.FailDialogue = from.FailDialogue;
            result.HelpSteps = from.HelpSteps;
            result.Stage = null;
            result.FailCount = 0;
            return result;
        }
    }
}