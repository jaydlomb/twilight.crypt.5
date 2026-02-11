using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI for the 'Home base'
/// This is both the main menu, and the area where players upgrade stuff
/// </summary>
public class HomeBaseUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Button startRunButton;

    [Header("Difficulty Selection")]
    [SerializeField] private TMP_Dropdown difficultyDropdown;

    [Header("Upgrades")]
    [SerializeField] private TextMeshProUGUI healthLevelText;
    [SerializeField] private TextMeshProUGUI healthCostText;
    [SerializeField] private Button healthUpgradeButton;

    [SerializeField] private TextMeshProUGUI resistanceLevelText;
    [SerializeField] private TextMeshProUGUI resistanceCostText;
    [SerializeField] private Button resistanceUpgradeButton;

    private void OnEnable()
    {
        RefreshUI();
    }

    private void Start()
    {
        // hook up buttons
        startRunButton?.onClick.AddListener(OnStartRunClicked);
        healthUpgradeButton?.onClick.AddListener(OnHealthUpgradeClicked);
        resistanceUpgradeButton?.onClick.AddListener(OnResistanceUpgradeClicked);

        PopulateDifficultyDropdown();
    }

    private void RefreshUI()
    {
        // update coin display
        if (coinText != null)
            coinText.text = $"Coins: {CoinManager.Instance.GetTotalCoins()}";

        // update health info
        if (healthLevelText != null)
            healthLevelText.text = $"Health Level: {UpgradeManager.Instance.GetHealthLevel()}";

        if (healthCostText != null)
            healthCostText.text = $"Cost: {UpgradeManager.Instance.GetHealthUpgradeCost()}";

        // update resistance info
        if (resistanceLevelText != null)
            resistanceLevelText.text = $"Resistance Level: {UpgradeManager.Instance.GetResistanceLevel()}";

        if (resistanceCostText != null)
            resistanceCostText.text = $"Cost: {UpgradeManager.Instance.GetResistanceUpgradeCost()}";

        PopulateDifficultyDropdown();
    }

    //make sure players can only select a difficulty they have gotten to
    // clear drop down and add all the options available to the player
    private void PopulateDifficultyDropdown()
    {
        if (difficultyDropdown == null) return;

        difficultyDropdown.ClearOptions();

        int highestUnlocked = GameManager.Instance.GetHighestUnlockedDifficulty();

        for (int i = 1; i <= highestUnlocked; i++)
        {
            difficultyDropdown.options.Add(new TMP_Dropdown.OptionData($"Difficulty {i}"));
        }

        difficultyDropdown.value = highestUnlocked - 1; // Default to highest unlocked
        difficultyDropdown.RefreshShownValue();
    }

    //start run
    private void OnStartRunClicked()
    {
        int selectedDifficulty = difficultyDropdown != null ? difficultyDropdown.value + 1 : 1;
        GameManager.Instance.StartRun(selectedDifficulty);
    }

    //attempt to upgrade health
    private void OnHealthUpgradeClicked()
    {
        if (UpgradeManager.Instance.TryUpgradeHealth())
        {
            RefreshUI();
            Debug.Log("Health upgraded!");
        }
        else
        {
            Debug.Log("Not enough coins for health upgrade!");
        }
    }

    //attempt to upgrade resistance
    private void OnResistanceUpgradeClicked()
    {
        if (UpgradeManager.Instance.TryUpgradeResistance())
        {
            RefreshUI();
            Debug.Log("Resistance upgraded!");
        }
        else
        {
            Debug.Log("Not enough coins for resistance upgrade!");
        }
    }
}