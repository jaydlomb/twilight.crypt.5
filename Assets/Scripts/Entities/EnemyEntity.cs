using UnityEngine;

/// <summary>
/// Enemy class used by all enemies
/// Children of base enemy class with animations, attacks, etc to come
/// </summary>
public class EnemyEntity : Entity
{
    [Header("Rewards")]
    [Tooltip("How much this enemy is worth")]
    [SerializeField] private int coinValue;

    //init
    public void InitializeStats(float health, int resistance, int coins)
    {
        maxHealth = health;
        currentHealth = health;
        resistanceLevel = resistance;
        coinValue = coins;
    }

    protected override void OnDeath()
    {
        // award coins
        CoinManager.Instance.AddCoins(coinValue);

        // tell RunManager enemy died
        RunManager.Instance.OnEnemyKilled();
        Destroy(gameObject);
    }
}
