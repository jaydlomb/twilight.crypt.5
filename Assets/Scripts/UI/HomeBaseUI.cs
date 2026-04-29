using DG.Tweening;
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
    [SerializeField] private Button quitButton;


    [Header("Difficulty Selection")]
    [SerializeField] private TMP_Dropdown difficultyDropdown;

    [Header("Upgrade Rows")]
    [SerializeField] private GameObject upgradeRowPrefab;
    [SerializeField] private RectTransform upgradeTitle;
    [SerializeField] private RectTransform upgradePanel;
    private List<UpgradeRowUI> rows = new List<UpgradeRowUI>();

    [Header("Stats Display")]
    [SerializeField] private GameObject statRowPrefab;
    [SerializeField] private RectTransform statsTitle;
    [SerializeField] private RectTransform statsPanel;
    private List<StatRowUI> statRows = new List<StatRowUI>();
    private bool initialized = false;

    private void OnEnable()
    {
        if (CoinManager.Instance != null)
            RefreshUI();
        if (initialized )
            AnimatePanels();
    }

    private void Start()
    {
        startRunButton?.onClick.AddListener(OnStartRunClicked);
        quitButton?.onClick.AddListener(OnQuitClicked);

        BuildUpgradeRows();
        BuildStatRows();
        PopulateDifficultyDropdown();
        RefreshUI();

        initialized = true;
        AnimatePanels();
    }

    private void BuildUpgradeRows()
    {
        foreach (StatDefine stat in UpgradeManager.Instance.GetAllStats())
        {
            GameObject rowObj = Instantiate(upgradeRowPrefab, upgradePanel);
            UpgradeRowUI row = rowObj.GetComponent<UpgradeRowUI>();
            row.Setup(stat, this);
            rows.Add(row);
        }
    }

    private void BuildStatRows()
    {
        foreach (StatDefine stat in UpgradeManager.Instance.GetAllStats())
        {
            GameObject rowObj = Instantiate(statRowPrefab, statsPanel);
            StatRowUI row = rowObj.GetComponent<StatRowUI>();
            row.Setup(stat);
            statRows.Add(row);
        }
    }

    public void RefreshUI()
    {
        if (coinText != null)
            coinText.text = $"Coins: {CoinManager.Instance.GetTotalCoins()}";

        foreach (UpgradeRowUI row in rows)
            row.Refresh();

        foreach (StatRowUI row in statRows)
            row.Refresh();

        PopulateDifficultyDropdown();
    }

    private void AnimatePanels()
    {
        Vector2 upgradeTitlePos = upgradeTitle.anchoredPosition;
        Vector2 upgradePanelPos = upgradePanel.anchoredPosition;
        Vector2 statsTitlePos = statsTitle.anchoredPosition;
        Vector2 statsPanelPos = statsPanel.anchoredPosition;

        // slide from left
        upgradeTitle.anchoredPosition = new Vector2(-Screen.width, upgradeTitlePos.y);
        upgradePanel.anchoredPosition = new Vector2(-Screen.width, upgradePanelPos.y);

        // slide from right
        statsTitle.anchoredPosition = new Vector2(Screen.width, statsTitlePos.y);
        statsPanel.anchoredPosition = new Vector2(Screen.width, statsPanelPos.y);

        upgradeTitle.DOAnchorPos(upgradeTitlePos, 0.6f).SetEase(Ease.OutBack);
        upgradePanel.DOAnchorPos(upgradePanelPos, 0.6f).SetEase(Ease.OutBack).SetDelay(0.1f);

        statsTitle.DOAnchorPos(statsTitlePos, 0.6f).SetEase(Ease.OutBack).SetDelay(0.05f);
        statsPanel.DOAnchorPos(statsPanelPos, 0.6f).SetEase(Ease.OutBack).SetDelay(0.15f);
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

    private void OnQuitClicked()
    {
        Application.Quit();
    }
}