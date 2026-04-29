using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordEntity : MonoBehaviour
{
    [Header("Swing Settings")]
    [SerializeField] private float swingDuration = 0.4f;

    private float currentDamage;
    private bool isSwinging;
    private HashSet<EnemyEntity> hitEnemies = new HashSet<EnemyEntity>();

    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        SetSwordActive(false);
    }

    public void StartSwing(float damage)
    {
        if (isSwinging) return;
        currentDamage = damage;
        StartCoroutine(SwingCoroutine());
    }

    private IEnumerator SwingCoroutine()
    {
        isSwinging = true;
        hitEnemies.Clear();
        SetSwordActive(true);

        float elapsed = 0f;
        float startAngle = transform.localEulerAngles.z;

        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            float angle = startAngle + (360f * (elapsed / swingDuration));
            transform.localEulerAngles = new Vector3(0f, 0f, angle);
            yield return null;
        }

        SetSwordActive(false);
        isSwinging = false;
    }

    private void SetSwordActive(bool active)
    {
        if (spriteRenderer != null) spriteRenderer.enabled = active;
        if (col != null) col.enabled = active;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyEntity enemy = other.GetComponent<EnemyEntity>();
        if (enemy != null && !hitEnemies.Contains(enemy))
        {
            hitEnemies.Add(enemy);
            enemy.TakeDamage(currentDamage);
        }
    }
}