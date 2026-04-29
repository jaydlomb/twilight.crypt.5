using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    //list of events
    public event Action<PlayerEntity> OnPlayerSpawned;
    public event Action<float> OnPlayerDamaged;


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

    public void PlayerSpawned(PlayerEntity player)
    {
        OnPlayerSpawned?.Invoke(player);
    }

    public void PlayerDamaged(float currentHealth)
    {
        OnPlayerDamaged?.Invoke(currentHealth);
    }
}