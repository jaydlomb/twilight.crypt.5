using UnityEngine;

/// <summary>
/// Base abstract clased inherited by Player and Enemy classes
/// </summary>
public abstract class Entity : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected int resistanceLevel;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        // calculate damage after resistance
        // res is an percentage
        float resistanceMultiplier = 1f - (resistanceLevel / 100f);
        float actualDamage = damageAmount * resistanceMultiplier;

        currentHealth -= actualDamage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath();
        }
    }

    protected abstract void OnDeath();

    //getters
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public int GetResistanceLevel() => resistanceLevel;
}
