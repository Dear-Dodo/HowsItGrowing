/*
 *  Author: Calvin Soueid
 *  Date:   10/11/2021
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Interfaces;

namespace Plant
{
    public class FertiliserBags : EnvironmentObject
    {
        public static FertiliserBags Instance;

        public MeshRenderer aboveground;
        public MeshRenderer underground;

        public ParticleSystem soilPuff;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                throw new System.Exception("Instance of FertiliserBags already exists!");
            }
            Instance = this;
        }
        [SerializeField]
        protected Interactable fertiliserBag;

        public int[] dirtColors;
        public Color[] particleColors;

        protected void Start()
        {
            //foreach (EnvironmentSelector<Fertiliser> s in Selectors)
            //{
            //    s.TriggerObject.OnInteracted.AddListener(() =>
            //    {
            //        if (isActive)
            //        {
            //            PlantEnvironment.Instance.CurrentFertilizer = s.TargetValue;
            //            aboveground.material.SetFloat("_Hue", dirtColors[Array.IndexOf(Selectors, s)]);
            //            underground.material.SetFloat("_Hue", dirtColors[Array.IndexOf(Selectors, s)]);
            //            soilPuff.startColor = particleColors[Array.IndexOf(Selectors, s)];
            //            soilPuff.Play();
            //        }
            //    });
            //}
            fertiliserBag.OnInteracted.AddListener(() =>
            {
                if (isActive)
                {
                    PlantEnvironment.Instance.CurrentFertilizer = PlantEnvironment.Instance.CurrentStage.Info.Requirements.Fertiliser;
                    SetDirty();
                    SetActive(false);
                }
            });
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
            fertiliserBag.SetActive(enabled);
        }
    } 
}
