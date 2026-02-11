using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles the logic after the player begins a run
/// deals with starting and ending a Run
/// </summary>
public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerEntity playerPrefab;
    [SerializeField] private EnemyEntity enemyPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform[] enemySpawnPoints;

    [Header("Run State")]
    private int currentDifficulty;
    private float survivalTimer;
    //private float poisonTickTimer;
    private RunStats currentRunStats;
    private List<Entity> allEntities = new List<Entity>();
    private PlayerEntity currentPlayer;

    private bool runActive = false;

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

    public void StartRun(int difficulty)
    {
        currentDifficulty = difficulty;
        runActive = true;

        // reset stats
        currentRunStats = new RunStats
        {
            coinsEarned = 0,
            enemiesKilled = 0,
            timeSurvived = 0f,
            didWin = false
        };

        // clear remaining entities
        ClearEntities();

        // get difficulty values
        survivalTimer = DifficultyManager.Instance.GetSurvivalTime(difficulty);
        //poisonTickTimer = DifficultyManager.Instance.GetPoisonTickInterval();

        // spawn player + enemies
        SpawnPlayer();
        SpawnEnemies();

        // countdown timer and poison
        StartCoroutine(PoisonTickCoroutine());
        StartCoroutine(SurvivalTimerCoroutine());
    }

    private void SpawnPlayer()
    {
        currentPlayer = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);

        float playerHealth = UpgradeManager.Instance.GetPlayerMaxHealth();
        int playerResistance = UpgradeManager.Instance.GetPlayerResistance();

        currentPlayer.InitializeStats(playerHealth, playerResistance);
        allEntities.Add(currentPlayer);
    }

    private void SpawnEnemies()
    {
        int enemyCount = DifficultyManager.Instance.GetEnemyCount(currentDifficulty);
        float enemyHealth = DifficultyManager.Instance.GetEnemyHealth(currentDifficulty);
        int enemyResistance = DifficultyManager.Instance.GetEnemyResistance(currentDifficulty);
        int coinValue = DifficultyManager.Instance.GetCoinsPerEnemy(currentDifficulty);

        for (int i = 0; i < enemyCount; i++)
        {
            // cycle through spawn points array
            Transform spawnPoint = enemySpawnPoints[i % enemySpawnPoints.Length];

            EnemyEntity enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemy.InitializeStats(enemyHealth, enemyResistance, coinValue);
            allEntities.Add(enemy);
        }
    }

    private IEnumerator PoisonTickCoroutine()
    {
        float tickInterval = DifficultyManager.Instance.GetPoisonTickInterval();
        float poisonDamage = DifficultyManager.Instance.GetPoisonDamage();

        while (runActive)
        {
            yield return new WaitForSeconds(tickInterval);

            // Apply poison to all entities
            foreach (Entity entity in allEntities)
            {
                if (entity != null)
                {
                    entity.TakeDamage(poisonDamage);
                }
            }
        }
    }

    private IEnumerator SurvivalTimerCoroutine()
    {
        while (survivalTimer > 0 && runActive)
        {
            yield return new WaitForSeconds(1f);
            survivalTimer--;
            currentRunStats.timeSurvived++;

            if (survivalTimer % 5 == 0)
                Debug.Log($"Timer: {survivalTimer}s remaining"); 
        }

        // timer reached 0 = player won
        if (runActive)
        {
            EndRun(true);
        }
    }

    //enemy dies
    public void OnEnemyKilled()
    {
        currentRunStats.enemiesKilled++;
        currentRunStats.coinsEarned += DifficultyManager.Instance.GetCoinsPerEnemy(currentDifficulty);
    }

    //end run either a LOSS or a WIN, coins awarded both ways
    public void EndRun(bool won)
    {
        runActive = false;
        currentRunStats.didWin = won;

        // add completion bonus if won
        if (won)
        {
            currentRunStats.coinsEarned += DifficultyManager.Instance.GetCompletionBonus(currentDifficulty);
        }

        StopAllCoroutines();
        GameManager.Instance.OnRunComplete(won, currentRunStats);
    }

    private void ClearEntities()
    {
        foreach (Entity entity in allEntities)
        {
            if (entity != null)
            {
                Destroy(entity.gameObject);
            }
        }
        allEntities.Clear();
        currentPlayer = null;
    }

    // getter for UI
    public float GetSurvivalTimer() => survivalTimer;
    public RunStats GetCurrentRunStats() => currentRunStats;
}