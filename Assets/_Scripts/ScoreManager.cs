using System;
using UnityEngine;
using Unity.Entities;

public class ScoreManager : MonoBehaviour
{

    public int score = 0;

    public static event Action OnBrickCollided;
    public static event Action OnBallDied;
    public static ScoreManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    public static void TriggerBallDied()
    {
        OnBallDied?.Invoke();
    }

    public static void TriggerOnBrickCollided()
    {
        Instance.score++;
        OnBrickCollided?.Invoke();
    }
}
    