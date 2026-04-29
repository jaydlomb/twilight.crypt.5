using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEntity : Entity
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Attack")]
    [SerializeField] private SwordEntity sword;

    private Vector2 moveInput;
    private float attackSpeed;
    private float attackDamage;
    private float attackTimer;

    private void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f).normalized;
        transform.position += movement * moveSpeed * Time.deltaTime;

        if (moveInput.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (moveInput.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            attackTimer = attackSpeed;
            sword.StartSwing(attackDamage);
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void InitializeStats()
    {
        maxHealth = UpgradeManager.Instance.GetValue("health");
        currentHealth = maxHealth;
        resistanceLevel = (int)UpgradeManager.Instance.GetValue("resistance");
        attackDamage = UpgradeManager.Instance.GetValue("attackDamage");
        attackSpeed = Mathf.Max(0.5f, 3f - (UpgradeManager.Instance.GetValue("attackSpeed") * 0.5f));
        attackTimer = attackSpeed;
    }

    protected override void OnDeath()
    {
        CameraShake.Instance.Shake();
        GameManager.Instance.OnPlayerDeath();
    }
}