/*
 * Author: Alex Manning
 * Date: 3/11/2021
 * Folder Location: Assets/Scripts/WaratahChallenge
 */

using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : Singleton<Spawner>
{
    public GameObject Seed;                     //Seed prefab to spawn
    public bool Running;                        //Boolean if the spawner is running
    public int Score;                           //Player score
    public float SpawnRate;                     //Rate of spawning seeds
    public float InstantiatePointA;             //First point of a line to spawn along
    public float InstantiatePointB;             //Last point of a line to spawn along
    public float TimeLimit;                     //Time limit for the game
    public float BonusTime;                     //Bonus time per seed collected
    public float PeanaltyTime;                  //Peanalty time per seed dropped
    public Canvas HUD;                          //HUD canvas
    public TextMeshProUGUI ScoreText;           //HUD score
    public TextMeshProUGUI TimerText;           //HUD timer

    public JournalPage RelatedPage;             //Plant journal page
    public GameObject GameOverScreen;           //Game over canvas
    public TextMeshProUGUI gameOverScoreText;   //Game over score

    public DoAlpha FadePanel;                   //Score animation
    public DoScale GifPanel;

    // Called When Game Ends (James)
    public UnityEvent OnGameOver;
    public UnityEvent OnGameStart;

    //time remaining
    private float timer;

    // Start is called before the first frame update
    private void Start()
    {
        // Calls Intro Sequence when Game Starts (James)
        //Instance.FadePanel.Run(0, false, BeginGame);
        Instance.GifPanel.PlayOnEnable = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Instance.timer > 0)
        {
            Instance.timer -= Time.deltaTime;
        }
        // Changed to ELIF to ensure this is only called once. (James)
        else if (Instance.Running)
        {
            Instance.Running = false;
            GameOver();
        }
        Instance.ScoreText.text = "Score: " + Score;
        Instance.TimerText.text = "Timer: " + timer.ToString("0.");
    }

    /// <summary>
    /// Spawns a seed between two points
    /// </summary>
    private void spawnSeed()
    {
        if (Instance.Running)
        {
            Vector3 pos = this.transform.position;
            pos.x += Random.Range(InstantiatePointA, InstantiatePointB);
            Seed seedObj = Instantiate(Seed, pos, Quaternion.identity).GetComponent<Seed>();
            seedObj.Spawner = this;
            Invoke(nameof(spawnSeed), SpawnRate);
        }
    }

    /// <summary>
    /// adds time to the timer
    /// </summary>
    public void addBonusTime()
    {
        Instance.timer += BonusTime;
    }

    /// <summary>
    /// removes time from the timer
    /// </summary>
    public void subtractPeanaltyTime()
    {
        Instance.timer -= PeanaltyTime;
    }

    public void Run()
    {
        Instance.FadePanel.Run(0, false, BeginGame);
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void BeginGame()
    {
        // Using UI Stack (James)
        Instance.FadePanel.gameObject.SetActive(false);
        UserInterfaceStack.Instance.Show("GameUI");

        Instance.Running = true;
        Instance.timer = TimeLimit;
        Invoke(nameof(spawnSeed), Instance.SpawnRate);

        OnGameStart?.Invoke();
    }

    /// <summary>
    /// ends the game
    /// </summary>
    private void GameOver()
    {
        // Invoking Event and Showing Game Over UI (James)
        OnGameOver?.Invoke();
        UserInterfaceStack.Instance.Show("GameOverUI", true);

        Instance.gameOverScoreText.text = Score.ToString();
        Cursor.visible = true;
        if (LockedPages.Pages.ContainsKey("Waratah"))
        {
            LockedPages.Pages["Waratah"] = false;
        }
        PlayerPrefs.SetInt("WaratahScore", Score);
    }
}