/*
 *  Author: Calvin Soueid
 *  Date:   27/10/2021
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using StateMachine;
using Types;

namespace Plant.Data
{
    public enum CameraPosition
    {
        Close,
        Far
    }

    [CreateAssetMenu(fileName = "Data", menuName = "Data/PlantInfo", order = 1)]
    public class PlantInfo : ScriptableObject
    {
        public string Name;
        public PlantStageInfo[] PlantStages;

        public string MinigameSceneName;
    }

}