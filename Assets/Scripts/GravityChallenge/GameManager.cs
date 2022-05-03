/*
 *  Author: James Greensill
 *  Folder Location: Assets/Scripts/Gravity Challenge
 */

using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace RanunculusChallenge
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public LineRenderer LineRenderer;

        public TextMeshProUGUI ScoreText;
        public TextMeshProUGUI SeedText;

        public TextMeshProUGUI GameOverScoreText;

        public int MaxSeedCount = 25;

        public JournalPage ChallengeRelatedPage;

        public Conversation StartConversation;

        public UnityEvent OnGameStart;
        public UnityEvent OnGameOver;

        public DoAlpha FadePanel;

        public bool Running = false;
        public int SeedCount = 25;
        [HideInInspector] public int CollectedSeedCount = 0;

        private SeedLauncher m_Launcher;
        private int m_Score;

        private void Awake()
        {
            // Checks for singleton reference.
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            // Caches the Launcher
            if (m_Launcher == null)
                m_Launcher = FindObjectOfType<SeedLauncher>();

            FadePanel.Run(0, false, BeginGame);
        }

        private void Update()
        {
            // Detects if its time for game over.
            if (CollectedSeedCount >= MaxSeedCount && Running)
            {
                GameOver();
            }
        }

        /// <summary>
        /// Displays the seed trajectory.
        /// </summary>
        public void ProjectionDrag()
        {
            if (!Running)
                return;
            if (GameManager.Instance.SeedCount <= 0)
            {
                return;
            }

            // Converts Launcher Position to Screen Position.
            Vector3 seedpos = Camera.main.WorldToScreenPoint(m_Launcher.transform.position);

            // Gets displacement vector
            Vector3 seedDisplacementFromMouse = new Vector3(seedpos.x - Input.mousePosition.x, seedpos.y - Input.mousePosition.y,
                0);

            // calculates final position of projections endpoint
            Vector3 finalPos = seedpos + seedDisplacementFromMouse;

            // sends the projections endpoint to the final position
            LineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(finalPos));

            // gets the mouse position with the seedpos z axis to prevent skewing.
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, seedpos.z);

            // sets the final line renderer position to the mousePosition converted to world space.
            LineRenderer.SetPosition(LineRenderer.positionCount - 1, Camera.main.ScreenToWorldPoint(mousePosition));
        }

        public void AddScore(int score)
        {
            m_Score += score;
        }

        public void RemoveScore(int score)
        {
            m_Score -= score;
        }

        /// <summary>
        /// Updates all the UI
        /// </summary>
        private void OnGUI()
        {
            ScoreText.text = m_Score.ToString();
            SeedText.text = SeedCount.ToString();
            GameOverScoreText.text = m_Score.ToString();
        }

        /// <summary>
        /// Called on game over.
        /// </summary>
        private void GameOver()
        {
            Running = false;
            Save();
            // Locks the Ranunculus Page.
            if (LockedPages.Pages.ContainsKey("Ranunculus"))
            {
                LockedPages.Pages["Ranunculus"] = false;
            }
            OnGameOver?.Invoke();
        }

        private void Save()
        {
            PlayerPrefs.SetInt("RanunculusScore", m_Score);
        }

        /// <summary>
        /// Called when the game begins.
        /// </summary>
        public void BeginGame()
        {
            OnGameStart?.Invoke();

            UserInterfaceStack.Instance.Show("DialogueUI");

            DialogueSystem.Instance.Play(StartConversation, false, false);
            DialogueSystem.Instance.onDialogueClose.AddListener(() =>
            {
                UserInterfaceStack.Instance.Show("GameUI");
                Running = true;
            });
        }

        /// <summary>
        /// Hides the Trajectory Projection.
        /// </summary>
        public void FadeProjection()
        {
            LineRenderer.SetPosition(LineRenderer.positionCount - 1, m_Launcher.transform.position);
            LineRenderer.SetPosition(0, m_Launcher.transform.position);
        }
    }
}