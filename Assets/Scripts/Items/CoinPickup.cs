using UnityEngine;
using DG.Tweening;

public class CoinPickup : MonoBehaviour
{
    private Transform coinUITarget;
    private int value;

    public void Initialize(int coinValue)
    {
        value = coinValue;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerEntity>() != null)
        {
            coinUITarget = GameObject.Find("CoinsText").transform;
            FlyToUI();
        }
    }

    private void FlyToUI()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Sprite coinSprite = sr.sprite;

        GameObject uiCoin = new GameObject("FlyingCoin");
        uiCoin.transform.SetParent(coinUITarget.transform.root, false);
        UnityEngine.UI.Image img = uiCoin.AddComponent<UnityEngine.UI.Image>();
        img.sprite = coinSprite;
        img.rectTransform.sizeDelta = new Vector2(30f, 30f);
        img.rectTransform.position = screenPos;

        sr.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        img.rectTransform.DOMove(coinUITarget.position, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                Collect();
                Destroy(uiCoin);
            });
    }

    public void Collect()
    {
        DOTween.Kill(GetComponent<SpriteRenderer>());
        RunManager.Instance.OnCoinCollected(value);
        Destroy(gameObject);
    }
}