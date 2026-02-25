using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Heart Display")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartContainer;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite threeQuarterHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite quarterHeart;
    [SerializeField] private Sprite emptyHeart;

    private List<Image> heartImages = new List<Image>();
    private const float HP_PER_HEART = 4f;
    private PlayerEntity player;

    private void OnEnable()
    {
        EventManager.Instance.OnPlayerSpawned += OnPlayerSpawned;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnPlayerSpawned -= OnPlayerSpawned;
    }

    private void Update()
    {
        UpdateUI();
    }

    private void OnPlayerSpawned(PlayerEntity playerEntity)
    {
        this.player = playerEntity;
        InitializeHearts();
    }

    private void InitializeHearts()
    {
        // clear old hearts
        foreach (Image heart in heartImages) 
        {
            if (heart != null) Destroy(heart.gameObject);
        }
        heartImages.Clear();

        if (player == null) return;

        // how many hearts needed
        int heartCount = Mathf.CeilToInt(player.GetMaxHealth() / HP_PER_HEART);
        Debug.Log($"Max Health: {player.GetMaxHealth()}, HP_PER_HEART: {HP_PER_HEART}, Heart Count: {heartCount}");

        // spawn hearts
        for (int i = 0; i < heartCount; i++)
        {
            GameObject heartObj = Instantiate(heartPrefab, heartContainer);
            Image heartImage = heartObj.GetComponent<Image>(); 
            heartImages.Add(heartImage);
        }
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

        // update hearts
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        if (player == null) return;

        float currentHP = player.GetCurrentHealth();

        for (int i = 0; i < heartImages.Count; i++)
        {
            float hpForThisHeart = currentHP - (i * HP_PER_HEART);

            if (hpForThisHeart >= HP_PER_HEART)
            {
                heartImages[i].sprite = fullHeart; // 4/4 HP
            }
            else if (hpForThisHeart >= HP_PER_HEART * 0.75f)
            {
                heartImages[i].sprite = threeQuarterHeart; // 3/4 HP
            }
            else if (hpForThisHeart >= HP_PER_HEART * 0.5f)
            {
                heartImages[i].sprite = halfHeart; // 2/4 HP
            }
            else if (hpForThisHeart > 0)
            {
                heartImages[i].sprite = quarterHeart; // 1/4 HP
            }
            else
            {
                heartImages[i].sprite = emptyHeart; // 0 HP
            }
        }
    }
}