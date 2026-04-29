using DG.Tweening;
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
    [SerializeField] private Image vignetteImage;
    private bool isVignettePulsing = false;

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
    private bool isShaking = false;
    private void OnEnable()
    {
        isVignettePulsing = false;
        vignetteImage.DOKill();
        vignetteImage.color = new Color(0, 0, 0, 0);
        isShaking = false;
        timerText.transform.DOKill();
        EventManager.Instance.OnPlayerSpawned += OnPlayerSpawned;
        EventManager.Instance.OnPlayerDamaged += OnPlayerDamaged;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnPlayerSpawned -= OnPlayerSpawned;
        EventManager.Instance.OnPlayerDamaged -= OnPlayerDamaged;
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

    private void OnPlayerDamaged(float currentHealth)
    {
        int heartIndex = Mathf.FloorToInt(currentHealth / HP_PER_HEART);
        heartIndex = Mathf.Clamp(heartIndex, 0, heartImages.Count - 1);
        heartImages[heartIndex].transform.DOShakePosition(0.3f, 5f, 20, 90, false, true);
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
        if (timerText != null)
        {
            float timeRemaining = RunManager.Instance.GetSurvivalTimer();
            timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}s";

            if (timeRemaining <= 4f && !isShaking)
            {
                isShaking = true;
                timerText.transform.DOShakePosition(99f, 5f, 20, 90, false, true);
            }
        }

        if (coinsText != null)
        {
            RunStats stats = RunManager.Instance.GetCurrentRunStats();
            coinsText.text = $"Coins: {stats.coinsEarned}";
        }

        UpdateHearts();
    }

    private void UpdateHearts()
    {
        if (player == null) return;

        float currentHP = player.GetCurrentHealth();
        float maxHP = player.GetMaxHealth();

        // vignette pulse
        if (currentHP / maxHP <= 0.2f && !isVignettePulsing)
        {
            isVignettePulsing = true;
            vignetteImage.DOFade(0.6f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
        else if (currentHP / maxHP > 0.2f && isVignettePulsing)
        {
            isVignettePulsing = false;
            vignetteImage.DOKill();
            vignetteImage.color = new Color(0, 0, 0, 0);
        }

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