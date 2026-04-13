using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button upgradeButton;

    private string statId;
    private HomeBaseUI parentUI;

    public void Setup(StatDefine stat, HomeBaseUI parent)
    {
        statId = stat.statId;
        parentUI = parent;
        nameText.text = stat.displayName;
        upgradeButton.onClick.AddListener(OnClicked);
        Refresh();
    }

    public void Refresh()
    {
        levelText.text = $"Level: {UpgradeManager.Instance.GetLevel(statId)}";
        costText.text = $"Cost: {UpgradeManager.Instance.GetUpgradeCost(statId)}";
    }

    private void OnClicked()
    {
        if (UpgradeManager.Instance.TryUpgrade(statId))
        {
            parentUI.RefreshUI();
        }
    }
}