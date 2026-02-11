using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player class used to control the player
/// </summary>
public class PlayerEntity : Entity
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 moveInput;

    private void Update()
    {
        HandleMovement();
    }

    //handle movement with new input system
    private void HandleMovement()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f).normalized;
        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    //called by new input system
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    //init
    public void InitializeStats(float health, int resistance)
    {
        maxHealth = health;
        currentHealth = health;
        resistanceLevel = resistance;
    }

    protected override void OnDeath()
    {
        //tell game manager player died, run ended
        GameManager.Instance.OnPlayerDeath();
        gameObject.SetActive(false);
    }
}
