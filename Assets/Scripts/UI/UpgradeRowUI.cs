using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UpgradeRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
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
        costText.text = $"Cost: {UpgradeManager.Instance.GetUpgradeCost(statId)}";
    }

    private void OnClicked()
    {
        if (UpgradeManager.Instance.TryUpgrade(statId))
        {
            upgradeButton.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1f);
            parentUI.RefreshUI();
        }
    }
}