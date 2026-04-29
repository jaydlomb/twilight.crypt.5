using UnityEngine;
using DG.Tweening;

/// <summary>
/// Base abstract clased inherited by Player and Enemy classes
/// </summary>
public abstract class Entity : MonoBehaviour
{
    [Header("Stats")]
    protected float maxHealth;
    protected float currentHealth;
    protected int resistanceLevel;

    private SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damageAmount)
    {
        float resistanceMultiplier = 1f - (resistanceLevel / 100f);
        float actualDamage = damageAmount * resistanceMultiplier;

        currentHealth -= actualDamage;
        FlashRed();

        if (this is PlayerEntity)
        {
            EventManager.Instance.PlayerDamaged(currentHealth);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath();
        }
    }

    private void FlashRed()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.DOColor(Color.red, 0.1f)
            .OnComplete(() => spriteRenderer.DOColor(Color.white, 0.1f));
    }

    protected abstract void OnDeath();

    //getters
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public int GetResistanceLevel() => resistanceLevel;
}
