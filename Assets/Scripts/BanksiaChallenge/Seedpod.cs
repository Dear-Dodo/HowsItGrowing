/*
 * Author: Ronan Richardson
 * Date: 17/11/2021
 * Folder Location: Assets/Scripts/BanksiaChallenge
 */

using UnityEngine;

// Disable the "never assigned to, default value of null" warning as it is bugged (all members are assigned a value)
#pragma warning disable 0649

namespace BanksiaChallenge
{
    /// <summary>
    /// Seedpod is the sole controller script attached to each seedpod prefab. It handles the local spawning of seeds that are held
    /// as a child at it's seedSpawnPoint transform, as well as triggering the activation logic for animating an opening seedpod.
    /// </summary>
    public class Seedpod : MonoBehaviour
    {
        [Header("The seed prefab to spawn within this seedpod, and the transform position to spawn it at")]
        [SerializeField] private GameObject m_seedPrefab;
        [SerializeField] private Transform m_seedSpawnPoint;
        
        [Header("The animation to play each time a seedpod becomes interactable and active.")]
        [SerializeField] private Animator m_seedpodAnimator;
        
        // A reference to the local seed spawned by this seedpod
        private Seed m_localSeed;

        /// <summary>
        /// Activate() first checks to see if this seedpod has a local seed already spawned, and spawns one if not,
        /// the function then initiates the local seeds movement locking to this seedpod and plays the open animation
        /// attached to this seedpod.
        /// </summary>
        public void Activate()
        {
            // If a local seed has yet to be spawned in this seedpod, spawn one
            if (m_localSeed == null)
            {
                SpawnSeed();
            }
            // Initalise the local movement lock on this seedpod's seed
            m_localSeed.InitMovementLock();

            // Play the open animation for this seedpod
            m_seedpodAnimator.SetBool("seedpodOpen", true);
        }

        /// <summary>
        /// SpawnSeed() simply instantiates a seedPrefab and stores it as this seedpod's localSeed.
        /// This function is called on Awake() and on Activate() if there is no local seed currently.
        /// </summary>
        public void SpawnSeed()
        {
            m_localSeed = Instantiate(m_seedPrefab, m_seedSpawnPoint).GetComponent<Seed>();
            m_localSeed.ParentPod = this;
        }
    }
}

