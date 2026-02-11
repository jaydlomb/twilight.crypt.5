using UnityEngine;

/// <summary>
/// Deals with player upgrades
/// ready to add on more upgrades
/// Current upgradable stats:
/// - Health
/// - Resistance Level
/// Holds the current upgrade levels
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Player Upgrades")]
    [SerializeField] private PlayerUpgradeData upgradeData;

    [Header("Base Stats")]
    [SerializeField] private float baseHealthPerLevel = 10f;
    [SerializeField] private int baseResistancePerLevel = 1;

    [Header("Upgrade Costs")]
    [SerializeField] private int healthUpgradeCost = 50;
    [SerializeField] private int resistanceUpgradeCost = 75;
    [SerializeField] private int costIncreasePerLevel = 25;

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
    }

    private void Start()
    {
        //init
        upgradeData.healthLevel = 1;
        upgradeData.resistanceLevel = 1;
    }

    public bool TryUpgradeHealth()
    {
        int cost = GetHealthUpgradeCost();
        if (CoinManager.Instance.TrySpendCoins(cost))
        {
            upgradeData.healthLevel++;
            return true;
        }
        return false;
    }

    public bool TryUpgradeResistance()
    {
        int cost = GetResistanceUpgradeCost();
        if (CoinManager.Instance.TrySpendCoins(cost))
        {
            upgradeData.resistanceLevel++;
            return true;
        }
        return false;
    }

    // cost of upgrade calculations
    public int GetHealthUpgradeCost()
    {
        return healthUpgradeCost + (costIncreasePerLevel * (upgradeData.healthLevel - 1));
    }

    public int GetResistanceUpgradeCost()
    {
        return resistanceUpgradeCost + (costIncreasePerLevel * (upgradeData.resistanceLevel - 1));
    }

    // get stat values
    public float GetPlayerMaxHealth()
    {
        return baseHealthPerLevel * upgradeData.healthLevel;
    }

    public int GetPlayerResistance()
    {
        return baseResistancePerLevel * upgradeData.resistanceLevel;
    }

    // getters for UI
    public int GetHealthLevel() => upgradeData.healthLevel;
    public int GetResistanceLevel() => upgradeData.resistanceLevel;
}

[System.Serializable]
public struct PlayerUpgradeData
{
    public int healthLevel;
    public int resistanceLevel;
}