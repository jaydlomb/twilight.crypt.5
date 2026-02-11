using UnityEngine;

/// <summary>
/// Currency manager for gaining, losing, AND spending coins
/// Used in the upgrade manager / menu
/// </summary>
public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    [Header("Currency")]
    [SerializeField] private int totalCoins = 0;

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

    public void AddCoins(int amount)
    {
        Debug.Log($"Coins: {amount} gained");
        totalCoins += amount;
    }

    //attempt to buy something
    public bool TrySpendCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            return true;
        }
        return false;
    }

    public int GetTotalCoins() => totalCoins;
}
