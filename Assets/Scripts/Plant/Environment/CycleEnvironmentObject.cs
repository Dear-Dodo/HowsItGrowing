/*
 *  Author: Calvin Soueid
 *  Date:   9/11/2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Plant
{
    /// <summary>
    /// Base class for environment objects that cycle value on click
    /// </summary>
    public abstract class CycleEnvironmentObject : EnvironmentObject
    {
        /// <summary>
        /// Object that is checked for interaction on this environment object's behalf.
        /// </summary>
        [SerializeField]
        [Tooltip("Object that is checked for interaction on this environment object's behalf")]
        protected Interactable triggerObject;

        /// <summary>
        /// Number of values within the backing enumerator.
        /// </summary>
        protected abstract int enumValueCount { get; }

        /// <summary>
        /// Incremement the value of the backing requirement.
        /// </summary>
        protected abstract void Cycle();

        protected virtual void Start()
        {
            triggerObject.OnInteracted.AddListener(Cycle);
        }

        public override void SetActive(bool enabled)
        {
            base.SetActive(enabled);
            triggerObject.SetActive(enabled);
        }
    } 
}
