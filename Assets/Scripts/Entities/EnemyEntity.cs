using DG.Tweening;
using UnityEngine;

public class EnemyEntity : Entity
{
    [Header("Rewards")]
    [Tooltip("How much this enemy is worth")]
    [SerializeField] private int coinValue;
    [SerializeField] private GameObject coinPrefab;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Vector2 bounceAreaMin = new Vector2(-8f, -4f);
    [SerializeField] private Vector2 bounceAreaMax = new Vector2(8f, 4f);

    private float contactDamage = 1f;

    private Vector2 moveDirection;

    public void InitializeStats(float health, int resistance, int coins, float contactDmg)
    {
        maxHealth = health;
        currentHealth = health;
        resistanceLevel = resistance;
        coinValue = coins;
        contactDamage = contactDmg;

        // random starting direction
        moveDirection = Random.insideUnitCircle.normalized;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        Vector3 pos = transform.position;

        // bounce on X
        if (pos.x <= bounceAreaMin.x || pos.x >= bounceAreaMax.x)
        {
            moveDirection.x *= -1f;
            pos.x = Mathf.Clamp(pos.x, bounceAreaMin.x, bounceAreaMax.x);
        }

        // bounce on Y
        if (pos.y <= bounceAreaMin.y || pos.y >= bounceAreaMax.y)
        {
            moveDirection.y *= -1f;
            pos.y = Mathf.Clamp(pos.y, bounceAreaMin.y, bounceAreaMax.y);
        }

        transform.position = pos;

        if (moveDirection.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    protected override void OnDeath()
    {
        DOTween.Kill(GetComponent<SpriteRenderer>());
        GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        coin.GetComponent<CoinPickup>().Initialize(coinValue);
        RunManager.Instance.OnEnemyKilled();
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerEntity player = other.GetComponent<PlayerEntity>();
        if (player != null)
        {
            player.TakeDamage(contactDamage);
            moveDirection = -moveDirection;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = new Vector3(
            (bounceAreaMin.x + bounceAreaMax.x) / 2f,
            (bounceAreaMin.y + bounceAreaMax.y) / 2f,
            0f);
        Vector3 size = new Vector3(
            bounceAreaMax.x - bounceAreaMin.x,
            bounceAreaMax.y - bounceAreaMin.y,
            0f);
        Gizmos.DrawWireCube(center, size);
    }
}