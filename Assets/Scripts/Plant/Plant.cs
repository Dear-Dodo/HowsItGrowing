/*
 *  Author: Calvin Soueid
 *  Date:   9/11/2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAsync;

namespace Plant
{
    public class Plant : MonoBehaviour
    {
        public static Plant Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                throw new System.Exception("An instance of Plant already exists");
            }
            Instance = this;
        }

        /// <summary>
        /// Instructs the plant to grow, playing animations/particles as necessary.
        /// Returns after completion of animations.
        /// </summary>
        public async void Grow()
        {
            throw new System.NotImplementedException();
        }
    }

}