using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

namespace Plant
{
    public class ShadeCloth : CycleEnvironmentObject
    {
        public static ShadeCloth Instance;

        public SkinnedMeshRenderer Cloth;
        private Animator animator;

        protected override int enumValueCount => System.Enum.GetValues(typeof(SunExposure)).Length;
        const int disabledOffset = 2;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                throw new System.Exception("Instance of ShadeCloth already exists!");
            }
            Instance = this;
        }

        protected override void Start()
        {
            animator = GetComponent<Animator>();
            UpdateBlendTarget();
            animator.SetFloat("Speed", 0);
            base.Start();
        }

        protected void Update()
        {
            Animate();
        }

        // Animation variables
        private float currentSpeed = 0;
        private float maxSpeed = 30;
        private float acceleration = 30;
        private float currentBlendTarget = 0;
        private float currentBlendValue;

        /// <summary>
        /// Control animations and blend shapes
        /// </summary>
        protected void Animate()
        {
            float distance = currentBlendTarget - currentBlendValue;
            if (Mathf.Abs(distance) < 0.5f)
            {
                currentSpeed = 0;
                currentBlendValue = currentBlendTarget;
                Cloth.SetBlendShapeWeight(0, currentBlendTarget);
                animator.SetFloat("Speed", 0);
                return;
            }

            // Constant Acceleration Formula rearranged to give max velocity is v^2 = u^2 + 2ax where u = final velocity, a = acceleration, and x = displacement (distance)
            float targetSpeed = Mathf.Clamp(Mathf.Sqrt(2 * acceleration * Mathf.Abs(distance)) * Mathf.Sign(distance), -maxSpeed, maxSpeed);
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
            currentBlendValue = currentBlendValue + currentSpeed * Time.deltaTime;

            // On exceeding bounds set speed to 0
            {
                float unclampedBlend = currentBlendValue;
                currentBlendValue = Mathf.Clamp(currentBlendValue, 0, 100);

                if (currentBlendValue != unclampedBlend)
                {
                    currentSpeed = 0;
                }
            }

            Cloth.SetBlendShapeWeight(0, currentBlendValue);
            animator.SetFloat("Speed", currentSpeed / maxSpeed * 2);
        }

        protected void UpdateBlendTarget()
        {
            currentBlendTarget = (100 / (enumValueCount - disabledOffset - 1)) * (int)PlantEnvironment.Instance.CurrentSunExposure;
        }

        protected override void Cycle()
        {
            if (isActive)
            {
                PlantEnvironment.Instance.CurrentSunExposure = (SunExposure)(((int)PlantEnvironment.Instance.CurrentSunExposure + 1) % (enumValueCount - disabledOffset));
                UpdateBlendTarget();
            }
        }

        public override void SetDirty()
        {
            UpdateBlendTarget();
        }

        public override void SetActive(bool enabled)
        {
            base.SetActive(enabled);
        }
    }

}