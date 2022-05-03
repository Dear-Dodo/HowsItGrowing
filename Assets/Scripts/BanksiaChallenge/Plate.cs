/*
 * Author: Ronan Richardson
 * Date: 14/11/2021
 * Folder Location: Assets/Scripts/BanksiaChallenge
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Disable the "never assigned to, default value of null" warning as it is bugged (all members are assigned a value)
#pragma warning disable 0649

namespace BanksiaChallenge
{
    /// <summary>
    /// Plate is the primary control class attached to the Plate prefab, and it handles moving the plate on and off screen,
    /// detecting the collection of seeds, as well as represeting the collection of seeds by activating preset seed prefabs
    /// that have been placed as if to be sitting on top of the plate. The class contains fields for adjusting the movement
    /// of the plate around the screen, as well as a field called m_plateSeeds which should hold the empty game object that
    /// contains all the inactive seed prefabs on the plate prefab.
    /// </summary>
    public class Plate : MonoBehaviour
    {
        // Reference to the plate's sparkle particles for seeds to trigger OnCollect
        public ParticleSystem SparkleParticles;

        // Variables for moving the plate on and off screen
        [Header("Variables that affect the cones arrival movement")]
        [SerializeField] private float m_arrivalTime = 0.3f;
        private Vector3 m_velocity = Vector3.zero;
        private Transform m_startingTarget;
        private Transform m_arrivalTarget;

        // Variables for activating and deactivating seed bodies on the plate
        [SerializeField] private GameObject m_plateSeeds;
        private Seed[] m_presetSeeds;
        private int m_nextActiveIdx;

        /// <summary>
        /// Start() simply initialises the starting and arrival targets for plate movement by search via tags in the scene,
        /// it will then start the MoveTo coroutine to move the plate from it's offscreen starting position, to it's onscreen
        /// arrival position. The function also caches the list of preset seed positions on the plate to display when a seed
        /// is dropped onto the plate.
        /// </summary>
        void Start()
        {
            if (m_startingTarget == null)
            {
                m_startingTarget = GameObject.FindGameObjectWithTag("PlateStartPosition").transform;
            }
            if (m_arrivalTarget == null)
            {
                m_arrivalTarget = GameObject.FindGameObjectWithTag("PlateEndPosition").transform;
            }

            // Cache the preset seeds that start invisible on the plate, and start at the 0th index
            m_presetSeeds = m_plateSeeds.GetComponentsInChildren<Seed>(true);
            m_nextActiveIdx = 0;
        }

        public void MoveOntoScreen()
        {
            StartCoroutine(MoveTo(m_startingTarget));
        }

        /// <summary>
        /// ActivateNextSeed() simply sets the next inactive seed in the list of presetSeeds to be active and visible.
        /// This has the effect of making it look as if seeds are being collected on the plate, and is called every
        /// time OnSeedCollected is invoked.
        /// </summary>
        public void ActivateNextSeed()
        {
            m_presetSeeds[m_nextActiveIdx].gameObject.SetActive(true);

            // If all seeds on the plate have been activated, reset the plate
            if (m_nextActiveIdx == m_presetSeeds.Length - 1)
            {
                StartCoroutine(Replate());
            }

            m_nextActiveIdx++;
        }

        /// <summary>
        /// MoveTo() is run as a coroutine that takes an input of a transform target to move to, and
        /// will lerp the position of this gameObject to that target using Vector3.SmoothDamp(), over 
        /// a time determined by the member variable arrivalTime. This function is called on start, and
        /// called at the end of every Replate() coroutine.
        /// </summary>
        /// <param name="moveToTarget">The transform of the target to move this gameObject towards.</param>
        IEnumerator MoveTo(Transform moveToTarget)
        {
            // While the distance between the current position and the target position is greater than 0.01units, SmoothDamp() to it
            while (Vector3.Distance(transform.position, moveToTarget.position) > 0.01f)
            {
                transform.position = Vector3.SmoothDamp(transform.position, moveToTarget.position, ref m_velocity, m_arrivalTime);
                yield return null;
            }
            // Set the transform position to equal the targets after movement has finished to stop over/undershooting
            transform.position = moveToTarget.position;

            yield return null;
        }

        /// <summary>
        /// Replate() is run as a coroutine that first moves the plate off screen, waits for 2 seconds, and then resets
        /// all of the presetSeeds on the plate to be inactive and invisible, effectively "emptying" the plate. The function
        /// will then move the plate back to it's original onscreen position.
        /// </summary>
        /// <returns></returns>
        IEnumerator Replate()
        {
            // Move the plate offscreen
            StartCoroutine(MoveTo(m_arrivalTarget));
            // Wait for it to be offscreen before replating
            yield return new WaitForSeconds(2);

            // For all of the presetSeeds, turn the inactive
            foreach (var seed in m_presetSeeds)
            {
                seed.gameObject.SetActive(false);
            }
            m_nextActiveIdx = 0;

            // Move the plate back onscreen
            StartCoroutine(MoveTo(m_startingTarget));
        }
    }
}