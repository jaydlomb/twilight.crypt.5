using UnityEngine;
using TMPro;

public class StatRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;

    private string statId;

    public void Setup(StatDefine stat)
    {
        statId = stat.statId;
        nameText.text = stat.displayName;
        Refresh();
    }

    public void Refresh()
    {
        levelText.text = $"{UpgradeManager.Instance.GetLevel(statId)}";
    }
}