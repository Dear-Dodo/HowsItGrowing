/*
 * Author: Ronan Richardson
 * Date: 23/11/2021
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
    /// GameManager is a manager class that handles triggering the intro cutscene and dialogue, the tutorial dialogue, as well as triggering gameplay.
    /// The manager also handles tracking player score and implementing the EndGame logic. The GameManager also handles playing
    /// and stopping the music for the scene.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnGameStart;

        [SerializeField] private MusicManagerHandler m_musicManagerHandler;
        // Music audio clips to play for the intro and gampeplay
        [SerializeField] private AudioClip m_startMusic;
        [SerializeField] private AudioClip m_gamePlayMusic;

        // Reference to BanksiaPage scriptable object to unlock it on EndGame() sequence
        [Header("Journal Page Scriptable Object for this challenge")]
        [SerializeField] private JournalPage m_banksiaJournalPage;

        private Camera m_mainCamera;
        [SerializeField] private Conversation m_introConversation;
        [SerializeField] private Conversation m_tutorialConversation;
        [SerializeField] private Animator m_ovenAnimator;
        [SerializeField] private Transform m_cameraIntroPosition;
        [SerializeField] private Transform m_cameraGameplayPosition;

        // Score parameters
        [Header("Runtime score-counter UI element")]
        [SerializeField] private TextMeshProUGUI m_scoreText;
        private int m_score;

        // End game UI parameters
        [Header("Endgame UI element")]
        [SerializeField] private TextMeshProUGUI m_endGameScoreText;

        // Reference to oven room object to deactivate on scene change
        [SerializeField] GameObject m_OvenRoom;

        /// <summary>
        /// Start() simply caches the main camera of the challenge scene, sets it's initial position
        /// to be the intro position transform attached in the inspector, and calls Play on the DialogueSystem
        /// instance to play the m_introConversation.
        /// </summary>
        private void Start()
        {
            m_score = 0;
            m_scoreText.text = m_score.ToString();

            DialogueSystem.Instance.onDialogueClose.AddListener(() => m_ovenAnimator.SetTrigger("cook"));
            m_mainCamera = Camera.main;
            m_mainCamera.transform.position = m_cameraIntroPosition.position;
            m_mainCamera.orthographic = false;

            // Trigger Wattle's intro conversation
            DialogueSystem.Instance.Play(m_introConversation);

            // Trigger the intro cutscene music
            m_musicManagerHandler.Stop();
            m_musicManagerHandler.Play(m_startMusic);
        }

        /// <summary>
        /// StartGame() first destroys the oven object so that it does not keep animating off screen.
        /// The function then moves the camera's position and orientation to be that of it's gameplay
        /// position. Finally the function calls Play on the Dialogue System instance to play the
        /// m_tutorialConversation. The function also adds two listeners that trigger the OnGameStart
        /// event and show's the gameplay menu onDialogueClose. This function is called by the
        /// CookingAnimEnd.cs script when the oven cooking animation has exited it's state.
        /// </summary>
        public void StartGame()
        {
            // Destroy the oven room and animator now that it is offscreen
            Destroy(m_ovenAnimator.gameObject);
            m_OvenRoom.SetActive(false);

            // Move the camera to it's gameplay position
            m_mainCamera.transform.position = m_cameraGameplayPosition.position;
            m_mainCamera.transform.Rotate(new Vector3(75, 0, 0), Space.World);
            m_mainCamera.orthographic = true;

            // Trigger the Banksia_Tweezer audio clip to play during gameplay
            m_musicManagerHandler.Stop();
            m_musicManagerHandler.Play(m_gamePlayMusic);

            // Trigger the tutorial conversation and add OnGameStart as a listener to onDialogueClose
            DialogueSystem.Instance.Play(m_tutorialConversation);
            DialogueSystem.Instance.onDialogueClose.AddListener(() => 
            { 
                UserInterfaceStack.Instance.Show("GameMenu");
                OnGameStart?.Invoke();
            });
        }

        /// <summary>
        /// EndGame() sets the BanksiaScore player pref with the current tracked score, and then
        /// activates the endgame panel, displaying how many seeds were collected and offering the
        /// user the option to return to the main scene. This function is triggered by the Timer's
        /// OnTimerEnd event.
        /// </summary>
        public void EndGame()
        {
            // Send the score to the BanksiaScore player pref
            PlayerPrefs.SetInt("BanksiaScore", m_score);

            // Unlock the banksia's challenge page in the main journal loop
            if (LockedPages.Pages.ContainsKey("Banksia"))
            {
                LockedPages.Pages["Banksia"] = false;
            }

            // Set the endgame score text using the tracked score, and activate the endgame panel gameObject
            m_endGameScoreText.text = m_score.ToString();
            UserInterfaceStack.Instance.Show("GameOverMenu", true);
        }

        private void OnBecameInvisible()
        {
            this.gameObject.SetActive(false);
        }


        /// <summary>
        /// IncreaseScore() simply increments the score tracker and updates
        /// the score text. The function is called by the plate object every
        /// time a seed is collected.
        /// </summary>



        public void IncreaseScore()
        {
            m_score++;
            m_scoreText.text = m_score.ToString();
        }
    }
}