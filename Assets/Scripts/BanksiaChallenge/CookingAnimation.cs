/*
 * Author: Ronan Richardson
 * Date: 23/11/2021
 * Folder Location: Assets/Scripts/BanksiaChallenge
 */

using UnityEngine;

namespace BanksiaChallenge
{
    /// <summary>
    /// CookingAnimEnd is a simple class that is attached as a behaviour to the cooking animation of
    /// the oven in the BanksiaChallenge scene. It contains one override function, OnStateExit,
    /// that is used to trigger the GameManager's StartGame() sequence once the intro cutscene
    /// animation has finished.
    /// </summary>
    public class CookingAnimation : StateMachineBehaviour
    {
        private ParticleSystem m_fireParticles;
        private SoundManager m_soundManager;

        /// <summary>
        /// OnStateEnter() is called when the oven first enters its cooking animation. This function
        /// simply triggers the fire particle system to play and triggers the BanksiaFire sound to play,
        /// which loops.
        /// </summary>
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Find the fire particle system attached to the oven and trigger it
            m_fireParticles = GameObject.Find("FireParticles").GetComponent<ParticleSystem>();
            m_fireParticles.Play();

            // Trigger the BanksiaFire Sound object in the sound manager
            m_soundManager = FindObjectOfType<SoundManager>();
            m_soundManager.Play("BanksiaFire3");
            m_soundManager.Play("BanksiaOvenTick");
        }

        /// <summary>
        /// OnStateExit() is called when the cooking animation of the oven finishes, and simply caches a
        /// reference to the GameManager in the scene and uses it to call GameManager.StartGame().
        /// </summary>
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.StartGame();

            m_soundManager.Stop("BanksiaFire3");
            m_soundManager.Stop("BanksiaOvenTick");
        }
    }
}