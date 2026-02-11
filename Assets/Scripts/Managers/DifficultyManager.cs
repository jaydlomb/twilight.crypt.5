using UnityEngine;

/// <summary>
/// Manages increasing difficulties
/// Uses Difficulty Scaling Data Scriptable object
/// Everything increases with the selected "Difficulty" level
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [Header("Scaling Data")]
    [SerializeField] private DifficultyScalingData scalingData;

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

    // enemy count increase
    public int GetEnemyCount(int difficulty)
    {
        return scalingData.baseEnemyCount + (scalingData.enemyCountPerDifficulty * (difficulty - 1));
    }

    //enemy health increase
    public float GetEnemyHealth(int difficulty)
    {
        return scalingData.baseEnemyHealth + (scalingData.enemyHealthPerDifficulty * (difficulty - 1));
    }

    //enemy resistance increase
    public int GetEnemyResistance(int difficulty)
    {
        return scalingData.baseEnemyResistance + (scalingData.enemyResistancePerDifficulty * (difficulty - 1));
    }

    // "air poison" damage - will be changed for actual enemy attacking
    public float GetPoisonDamage()
    {
        return scalingData.basePoisonDamage;
    }

    // air poison tick speed
    public float GetPoisonTickInterval()
    {
        return scalingData.poisonTickInterval;
    }

    // game timer
    public float GetSurvivalTime(int difficulty)
    {
        return scalingData.baseSurvivalTime + (scalingData.survivalTimePerDifficulty * (difficulty - 1));
    }

    // reward per enemy
    public int GetCoinsPerEnemy(int difficulty)
    {
        return scalingData.baseCoinsPerEnemy + (scalingData.coinsPerEnemyPerDifficulty * (difficulty - 1));
    }

    // rreward for beating level
    public int GetCompletionBonus(int difficulty)
    {
        return scalingData.baseCompletionBonus + (scalingData.completionBonusPerDifficulty * (difficulty - 1));
    }
}