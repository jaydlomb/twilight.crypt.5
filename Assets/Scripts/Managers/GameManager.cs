using UnityEngine;

/// <summary>
/// Manages the overal game flow
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("State")]
    [SerializeField] private GameState currentState;

    [Header("Difficulty")]
    [SerializeField] private int currentDifficulty = 1;
    [SerializeField] private int highestUnlockedDifficulty = 1;

    [Header("UI References")]
    [SerializeField] private HomeBaseUI homeBaseUI;
    [SerializeField] private RunUI runUI;
    [SerializeField] private RunEndUI runEndUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //start game
    private void Start()
    {
        SetState(GameState.HomeBase);
        Invoke("TestRun", 2f);
    }

    //auto start game until home base UI is in
    private void TestRun()
    {
        StartRun(1); // Start difficulty 1
    }

    //setting state of the game
    public void SetState(GameState newState)
    {
        currentState = newState;

        // hide ALL UI
        homeBaseUI.gameObject.SetActive(false);
        runUI.gameObject.SetActive(false);
        runEndUI.gameObject.SetActive(false);

        // show only necessary UI
        switch (currentState)
        {
            case GameState.HomeBase:
                homeBaseUI.gameObject.SetActive(true);
                break;
            case GameState.InRun:
                runUI.gameObject.SetActive(true);
                break;
            case GameState.RunEnd:
                runEndUI.gameObject.SetActive(true);
                break;
        }
    }

    //start a run
    public void StartRun(int difficulty)
    {
        currentDifficulty = difficulty;
        SetState(GameState.InRun);
        RunManager.Instance.StartRun(difficulty);
    }

    //when game is LOST or WON
    public void OnRunComplete(bool won, RunStats stats)
    {
        // awar coins
        CoinManager.Instance.AddCoins(stats.coinsEarned);

        // unlock next difficulty if won
        if (won && currentDifficulty == highestUnlockedDifficulty)
        {
            highestUnlockedDifficulty++;
        }

        // show ending UI
        SetState(GameState.RunEnd);
        runEndUI.ShowResults(won, stats);
    }

    //end the run if player dies
    //open up run end UI here
    public void OnPlayerDeath()
    {
        RunManager.Instance.EndRun(false);
    }

    //back to home screen UI
    public void ReturnToHomeBase()
    {
        SetState(GameState.HomeBase);
    }

    // getters
    public int GetCurrentDifficulty() => currentDifficulty;
    public int GetHighestUnlockedDifficulty() => highestUnlockedDifficulty;
    public GameState GetCurrentState() => currentState;
}

// state of the game enumerator
public enum GameState
{
    HomeBase,
    InRun,
    RunEnd
}