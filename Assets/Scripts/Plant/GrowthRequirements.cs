/*
 *  Author: Calvin Soueid
 *  Date:   27/10/2021
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plant
{


    /// <summary>
    /// Levels of SoilDepth.
    /// </summary>
    public enum SoilDepth
    {
        Disabled = -2,
        Uninteractable,
        Shallow,
        Moderate,
        Deep
    }

    /// <summary>
    /// Different soil types.
    /// </summary>
    public enum SoilType
    {
        Disabled = -2,
        Uninteractable,
        Sand,
        Sphagnuum,
        Acidic,
    }

    /// <summary>
    /// Amount of watering.
    /// </summary>
    public enum WaterLevel
    {
        Disabled = -2,
        Uninteractable,
        Sparing,
        Regular,
        Heavy
    }

    /// <summary>
    /// Amount of sun exposure.
    /// </summary>
    public enum SunExposure
    {
        Disabled = -2,
        Uninteractable,
        Shade,
        PartialSun,
        FullSun
    }

    /// <summary>
    /// Typs of fertiliser.
    /// </summary>
    public enum Fertiliser
    {
        Disabled = -2,
        Uninteractable,
        Compost,
        IronSulphate,
        Volcanic
    }
    public enum Pollinator
    {
        Disabled = -2,
        Uninteractable,
        SelfPollinating,
        Bees,
        Birds
    }

    public enum Dispersal
    { 
        Disabled = -2,
        Uninteractable,
        Wind,
        Gravity,
        Heat
    }

    /// <summary>
    /// Contains data about the requirements for a single plant stage to progress.
    /// </summary>
    // This could be more generic but would then be more difficult for people to interact with.
    [Serializable]
    public struct StageRequirement
    {
        public SoilDepth SoilDepth;
        public SoilType SoilType;
        public WaterLevel WaterLevel;
        public SunExposure SunExposure;
        public Fertiliser Fertiliser;
        public Pollinator Pollinator;
        public Dispersal DispersalMechanism;

        public StageRequirement(SoilDepth sd, SoilType st, WaterLevel wl, SunExposure se, Fertiliser f, Pollinator p, Dispersal d)
        {
            SoilDepth = sd;
            SoilType = st;
            WaterLevel = wl;
            SunExposure = se;
            Fertiliser = f;
            Pollinator = p;
            DispersalMechanism = d;
        }
    }
}


