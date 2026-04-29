using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Shake(float duration = 0.5f, float strength = 0.3f, int vibrato = 20)
    {
        transform.DOShakePosition(duration, strength, vibrato, 90, false, true);
    }
}