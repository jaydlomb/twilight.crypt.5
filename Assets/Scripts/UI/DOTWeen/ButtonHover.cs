using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleAmount = 1.1f;
    [SerializeField] private float duration = 0.15f;

    private Vector3 originalScale;
    public void Awake()
    {
        originalScale = transform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(scaleAmount*originalScale, duration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, duration).SetEase(Ease.OutBack);
    }
}