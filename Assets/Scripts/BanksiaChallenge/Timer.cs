/*
 * Author: Ronan Richardson
 * Date: 1/11/2021
 * Folder Location: Assets/Scripts/BanksiaChallenge
 */

using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Disable the "never assigned to, default value of null" warning as it is bugged (all members are assigned a value)
#pragma warning disable 0649

namespace BanksiaChallenge
{
    /// <summary>
    /// Timer is a simple timer class that starts from a given starting time, and will count down towards 0 each frame.
    /// The class has a text field for displaying the remaining time to the screen, and an OnTimerEnd event that invokes
    /// once the timer reaches 0, which is used for triggering the EndGame() sequence in the ScoreManager. The class also
    /// has methods for Increasing and Decreasing the current remaining time by a set amount, which are called when a seed
    /// collides with the plate or the table respectively.
    /// </summary>
    public class Timer : MonoBehaviour
    {
        [Header("Attach the timer UI element here")]
        [SerializeField] private TextMeshProUGUI m_TimerText;

        [Header("Timer Properties")]
        [SerializeField] private float m_StartingTime = 30;

        [SerializeField] private float m_IncreaseAmount = 0;
        [SerializeField] private float m_DecreaseAmount = 0;

        private float m_TimerCounter;
        private bool m_IsRunning;

        public UnityEvent OnTimerEnd;

        /// <summary>
        /// Update() will, if the timer is still running, decrease the timer counter each frame by the time passed,
        /// display the remaining time to the timerText component, and if the timer has run out it invokes the
        /// OnTimerEnd event (which triggers the EndGame() sequence in the ScoreManager).
        /// </summary>
        private void Update()
        {
            if (!m_IsRunning)
                return;
            // Decrease the counter by the time passed and display the remaining time to the screen
            m_TimerCounter -= Time.deltaTime;

            // Check to see if m_timer counter is nearly a whole int, if so update the timer text

            // If the timer has now run out, invoke OnTimerEnd
            if (!(m_TimerCounter <= 0))
                return;

            m_IsRunning = false;
            m_TimerText.gameObject.SetActive(false);
            OnTimerEnd?.Invoke();
        }

        private void OnGUI()
        {
            if (m_TimerText)
            {
                m_TimerText.text = $"{m_TimerCounter:0} seconds remaining.";
            }
        }

        /// <summary>
        /// StartTimer() simply sets the timer counter to be the set starting time, and
        /// initialises isRunning to true to start the timer counting down. This function
        /// is called at the end of the intro sequence run by the GameManager.
        /// </summary>

        public void StartTimer()
        {
            m_TimerCounter = m_StartingTime;
            m_IsRunning = true;
        }

        /// <summary>
        /// Increase() simply increases the current timerCount by the specified increase amount.
        /// It is called whenever a seed hits the plate and is collected.
        /// </summary>
        public void Increase()
        {
            m_TimerCounter += m_IncreaseAmount;
        }

        /// <summary>
        /// Decrease() simply decreases the current timerCount by the specified decreases amount.
        /// It is called whenever a seed hits anything other than the plate.
        /// </summary>
        public void Decrease()
        {
            m_TimerCounter -= m_DecreaseAmount;
        }
    }
}