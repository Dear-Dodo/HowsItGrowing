/*
 *  Author: Calvin Soueid
 *  Date:   10/11/2021
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interfaces;

namespace Plant
{
    public class DispersalMechanisms : EnvironmentObject
    {
        
        public static DispersalMechanisms Instance;

#pragma warning disable 0649

        [Serializable]
        struct ButtonEnvironmentSelector
        {
            public Button Trigger;
            public Dispersal Target;
        }

        [SerializeField]
        private ButtonEnvironmentSelector[] Selectors;

#pragma warning restore 0649

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                throw new System.Exception("Instance of DispersalMechanisms already exists!");
            }
            Instance = this;
        }

        protected virtual void Start()
        {
            foreach(ButtonEnvironmentSelector selector in Selectors)
            {
                selector.Trigger.onClick.AddListener(() => { 
                    if (isActive)
                    {
                        SetDispersal(selector.Target);
                    }
                });
            }
        }

        public void SetDispersal(Dispersal target)
        {
            PlantEnvironment.Instance.CurrentDispersal = target;
            PlantEnvironment.Instance.CheckConditions();
        }

        public override void SetDirty()
        {
            //TODO add feedback
        }

        public override void SetActive(bool enabled)
        {
            base.SetActive(enabled);
        }
    } 
}
