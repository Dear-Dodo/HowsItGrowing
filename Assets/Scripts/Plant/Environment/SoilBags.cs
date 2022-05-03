/*
 *  Author: Calvin Soueid
 *  Date:   10/11/2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using System;

namespace Plant
{

    public class SoilBags : EnvironmentObject
    {
        public static SoilBags Instance;
        public MeshRenderer aboveground;
        public MeshRenderer underground;
        public ParticleSystem soilPuff;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                throw new System.Exception("Instance of SoilBags already exists!");
            }
            Instance = this;
        }

        [SerializeField]
        protected EnvironmentSelector<SoilType>[] Selectors;

        public int[] dirtColors;
        public Color[] particleColors;

        protected void Start()
        {
            foreach (EnvironmentSelector<SoilType> s in Selectors)
            {
                s.TriggerObject.OnInteracted.AddListener(() =>
                {
                    if (isActive)
                    {
                        PlantEnvironment.Instance.CurrentSoilType = s.TargetValue;
                        aboveground.material.SetFloat("_Hue", dirtColors[Array.IndexOf(Selectors, s)]);
                        underground.material.SetFloat("_Hue", dirtColors[Array.IndexOf(Selectors, s)]);
                        soilPuff.startColor = particleColors[Array.IndexOf(Selectors, s)];
                        soilPuff.Play();
                    }
                });
            }
        }

        public override void SetDirty()
        {
            aboveground.material.SetFloat("_Hue", dirtColors[(int)PlantEnvironment.Instance.CurrentSoilType]);
            underground.material.SetFloat("_Hue", dirtColors[(int)PlantEnvironment.Instance.CurrentSoilType]);
            soilPuff.startColor = particleColors[(int)PlantEnvironment.Instance.CurrentSoilType];
            soilPuff.Play();
        }

        public override void SetActive(bool enabled)
        {
            base.SetActive(enabled);
        }
    }

}