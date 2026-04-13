using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Upgrade Rows")]
    [SerializeField] private GameObject upgradeRowPrefab;
    [SerializeField] private Transform upgradeRowContainer;

    private List<UpgradeRowUI> rows = new List<UpgradeRowUI>();

    private void OnEnable()
    {
        if (CoinManager.Instance != null)
            RefreshUI();
    }

    private void Start()
    {
        startRunButton?.onClick.AddListener(OnStartRunClicked);
        BuildUpgradeRows();
        PopulateDifficultyDropdown();
        RefreshUI();
    }

    private void BuildUpgradeRows()
    {
        foreach (StatDefine stat in UpgradeManager.Instance.GetAllStats())
        {
            GameObject rowObj = Instantiate(upgradeRowPrefab, upgradeRowContainer);
            UpgradeRowUI row = rowObj.GetComponent<UpgradeRowUI>();
            row.Setup(stat, this);
            rows.Add(row);
        }
    }

    public void RefreshUI()
    {
        if (coinText != null)
            coinText.text = $"Coins: {CoinManager.Instance.GetTotalCoins()}";

        foreach (UpgradeRowUI row in rows)
            row.Refresh();

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
}