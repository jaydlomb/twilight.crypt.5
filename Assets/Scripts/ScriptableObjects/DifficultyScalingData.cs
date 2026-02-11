using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyScalingData", menuName = "Scriptable Objects/DifficultyScalingData")]
public class DifficultyScalingData : ScriptableObject
{
    [Header("Enemy Scaling")]
    public int baseEnemyCount = 3;
    public int enemyCountPerDifficulty = 2;
    public float baseEnemyHealth = 3f;
    public float enemyHealthPerDifficulty = 1f;
    public int baseEnemyResistance = 2;
    public int enemyResistancePerDifficulty = 1;

    [Header("Air Poison")]
    public float basePoisonDamage = 0.5f;
    public float poisonTickInterval = 2f;

    [Header("Timer")]
    public float baseSurvivalTime = 20f;
    public float survivalTimePerDifficulty = 20f;

    [Header("Rewards")]
    public int baseCoinsPerEnemy = 5;
    public int coinsPerEnemyPerDifficulty = 2;
    public int baseCompletionBonus = 50;
    public int completionBonusPerDifficulty = 25;
}