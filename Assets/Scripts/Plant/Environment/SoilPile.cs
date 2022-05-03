/*
 *  Author: Calvin Soueid
 *  Date:   9/11/2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

namespace Plant
{
    public class SoilPile : EnvironmentObject
    {
        public static SoilPile Instance;

        public MeshFilter DirtPile;
        public Mesh[] DirtPileMeshes;
        public Transform SeedHole;

        public bool IsSetToCorrectValue = false;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                throw new System.Exception("Instance of SoilPile already exists!");
            }
            Instance = this;
        }

        protected void Start()
        {
            SetDirty();
        }

        protected int enumValueCount { get; } = System.Enum.GetValues(typeof(SoilDepth)).Length;

        public void SetToCorrectValue()
        {
            IsSetToCorrectValue = true;
            PlantEnvironment.Instance.CurrentSoilDepth = PlantEnvironment.Instance.CurrentStage.Info.Requirements.SoilDepth;
            SetDirty();
            SetActive(false);
        }

        public override void SetDirty()
        {
            if (IsSetToCorrectValue || PlantEnvironment.Instance?.CurrentSoilDepth == PlantEnvironment.Instance?.CurrentStage?.Info.Requirements.SoilDepth)
            {
                DirtPile.mesh = DirtPileMeshes[Mathf.Clamp((int)PlantEnvironment.Instance.CurrentSoilDepth, 0, 3)];
                SeedHole.transform.localPosition = new Vector3(SeedHole.transform.localPosition.x, -1.5f - (0.3f * (int)PlantEnvironment.Instance.CurrentSoilDepth), SeedHole.transform.localPosition.z);
                DirtPile.gameObject.SetActive(true);
                SeedHole.gameObject.SetActive(true);
            }
            else
            {
                DirtPile.gameObject.SetActive(false);
                SeedHole.gameObject.SetActive(false);
            }

        }

        public override void SetActive(bool enabled)
        {
            base.SetActive(enabled);
        }
    } 
}
