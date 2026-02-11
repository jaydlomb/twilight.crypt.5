using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI that is present within a run:
/// - health
/// - coins
/// - timer
/// </summary>
public class RunUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    private PlayerEntity player;

    private void OnEnable()
    {
        //need to redo this in a better way
        //find object of type is ass trash dont use
        player = FindObjectOfType<PlayerEntity>();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        // timer
        if (timerText != null)
        {
            float timeRemaining = RunManager.Instance.GetSurvivalTimer();
            timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}s";
        }

        // coins earned
        if (coinsText != null)
        {
            RunStats stats = RunManager.Instance.GetCurrentRunStats();
            coinsText.text = $"Coins: {stats.coinsEarned}";
        }

        // health bar slider
        if (player != null)
        {
            if (healthBar != null)
            {
                healthBar.maxValue = player.GetMaxHealth();
                healthBar.value = player.GetCurrentHealth();
            }

            if (healthText != null)
            {
                healthText.text = $"HP: {Mathf.CeilToInt(player.GetCurrentHealth())} / {Mathf.CeilToInt(player.GetMaxHealth())}";
            }
        }
    }
}