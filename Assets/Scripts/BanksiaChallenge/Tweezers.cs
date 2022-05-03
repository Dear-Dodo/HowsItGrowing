/*
 * Author: Ronan Richardson
 * Date: 11/11/2021
 * Folder Location: Assets/Scripts/BanksiaChallenge
 */

using UnityEngine;

// Disable the "never assigned to, default value of null" warning as it is bugged (all members are assigned a value)
#pragma warning disable 0649

namespace BanksiaChallenge
{
    /// <summary>
    /// Tweezers is a class that is used to control the position and animations of the Tweezers prefab. Every update,
    /// the script sets the Tweezers object's transform to be that of the current mouse position, and checks for user
    /// input to trigger the tweezer's grabbing and releasing animations.
    /// </summary>
    public class Tweezers : MonoBehaviour
    {
        // A reference to the scene's main camera for ScreenToWorld conversions on the mouse
        private Camera m_mainCamera;
        // A reference to the Tweezer prefab's animator for triggering grabbing and releasing onClick
        private Animator m_tweezerAnimator;
        // Used to cache the mouse position every FixedUpdate
        private Vector3 m_mousePosition;

        /// <summary>
        /// Start() simply initialises the member variables of the script
        /// </summary>
        void Start()
        {
            m_mainCamera = Camera.main;
            m_tweezerAnimator = GetComponentInChildren<Animator>();
        }

        /// <summary>
        /// FixedUpdate() simply gets the mouse position at a rate determined
        /// by the fixed time step, and sets the tweezer position to it.
        /// </summary>
        private void FixedUpdate()
        {
            // Get the mouse position and give it a y-value of 2
            m_mousePosition = m_mainCamera.ScreenToWorldPoint(Input.mousePosition);
            m_mousePosition.y = 2f;
            // Set the tweezers position to be that of the mouse every update
            transform.position = m_mousePosition;
        }

        /// <summary>
        /// Update() will each frame check for whether the user has pressed or released left click this frame, 
        /// and sets the appropriate animation trigger if an input is detected.
        /// </summary>
        void Update()
        {
            // If the user is currently holding down left click, set the grabbed trigger to true
            if (Input.GetMouseButton(0))
            {
                m_tweezerAnimator.SetTrigger("grabbed");
            }
            // Other wise if the user has just released left click this frame, set the released trigger to true and clear the grabbed trigger
            else if (Input.GetMouseButtonUp(0))
            {
                m_tweezerAnimator.ResetTrigger("grabbed");
                m_tweezerAnimator.SetTrigger("released");
            }
        }
    }
}