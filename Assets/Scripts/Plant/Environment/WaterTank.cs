/*
 *  Author: Calvin Soueid
 *  Date:   9/11/2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interfaces;

namespace Plant
{
    public class WaterTank : CycleEnvironmentObject
    {
        public static WaterTank Instance;

        public SkinnedMeshRenderer levelDisplay;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                throw new System.Exception("Instance of WaterTank already exists!");
            }
            Instance = this;
        }

        protected override void Start()
        {
            SetDirty();
            base.Start();
        }

        protected override int enumValueCount { get; } = System.Enum.GetValues(typeof(WaterLevel)).Length;

        const int disabledOffset = 2;

        protected void Update()
        {
            Animation();
        }

        private float targetBlendValue = 0;
        private float currentBlendValue = 0;
        private const float animationSpeed = 40;

        protected void Animation()
        {
            currentBlendValue = Mathf.MoveTowards(currentBlendValue, targetBlendValue, animationSpeed * Time.deltaTime);
            levelDisplay.SetBlendShapeWeight(0, currentBlendValue);
        }

        protected override void Cycle()
        {
            if (isActive)
            {
                PlantEnvironment.Instance.CurrentWaterLevel = (WaterLevel)(((int)PlantEnvironment.Instance.CurrentWaterLevel + 1) % (enumValueCount - disabledOffset));
                SetDirty();
            }
        }

        public override void SetDirty()
        {
            targetBlendValue = 100 - ((int)PlantEnvironment.Instance.CurrentWaterLevel) / ((float)enumValueCount - (float)disabledOffset - 1) * 100;
        }

        public override void SetActive(bool enabled)
        {
            base.SetActive(enabled);
        }
    } 
}
