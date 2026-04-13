using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deals with player upgrades
/// Current upgradable stats:
/// - Health
/// - Resistance Level
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Stat Definitions")]
    [SerializeField] private List<StatDefine> stats = new List<StatDefine>();

    private Dictionary<string, int> statLevels = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // init all stat levels to 1
        foreach (StatDefine stat in stats)
        {
            statLevels[stat.statId] = 1;
        }
    }

    public bool TryUpgrade(string statId)
    {
        StatDefine stat = GetStat(statId);
        if (stat == null) return false;

        int cost = GetUpgradeCost(statId);
        if (CoinManager.Instance.TrySpendCoins(cost))
        {
            statLevels[statId]++;
            return true;
        }
        return false;
    }
    public int GetLevel(string statId)
    {
        return statLevels.ContainsKey(statId) ? statLevels[statId] : 0;
    }

    public float GetValue(string statId)
    {
        StatDefine stat = GetStat(statId);
        if (stat == null) return 0f;
        return stat.baseValuePerLevel * GetLevel(statId);
    }

    public int GetUpgradeCost(string statId)
    {
        StatDefine stat = GetStat(statId);
        if (stat == null) return 0;
        return stat.baseUpgradeCost + (stat.costIncreasePerLevel * (GetLevel(statId) - 1));
    }

    public StatDefine GetStat(string statId)
    {
        return stats.Find(s => s.statId == statId);
    }

    public List<StatDefine> GetAllStats() => stats;
}