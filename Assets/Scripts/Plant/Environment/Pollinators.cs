/*
 *  Author: Calvin Soueid
 *  Date:   10/11/2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

namespace Plant
{

    public class Pollinators : EnvironmentObject
    {
        public static Pollinators Instance;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                throw new System.Exception("Instance of Pollinators already exists!");
            }
            Instance = this;
        }

        [SerializeField]
        protected EnvironmentSelector<Pollinator>[] Selectors;

        protected void Start()
        {
            foreach(EnvironmentSelector<Pollinator> s in Selectors)
            {
                s.TriggerObject.OnInteracted.AddListener(() =>
                {
                    if (isActive)
                    {
                        PlantEnvironment.Instance.CurrentPollinator = s.TargetValue; 
                    }
                });
            }
        }

        public override void SetDirty()
        {
            //TODO add feedback
        }

        public override void SetActive(bool enabled)
        {

            foreach (EnvironmentSelector<Pollinator> s in Selectors)
            {
                s.TriggerObject.SetActive(enabled);
            }
            base.SetActive(enabled);
        }
    }

}