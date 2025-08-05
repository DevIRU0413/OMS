using System;

using UnityEngine;

public class YSJ_GameManager : YSJ_SimpleSingleton<YSJ_GameManager>, IManager
{
    #region IManager
    public bool IsDontDestroy => isDontDestroyOnLoad;

    public void Cleanup() { }
    public GameObject GetGameObject() => this.gameObject;
    public void Initialize() { }

    #endregion

    [field: Header("플레이어 상태")]
    [field: SerializeField] private int maxHealth = 100;
    [field: SerializeField] public int PlayerHealth { get; private set; } = 100;
    [field: SerializeField] public float BatteryPercent { get; private set; } = 100f;
    [field: SerializeField] public float batteryDrainRate { get; private set; } = 1f; // 초당 1% 감소
   
    [field: Header("게임 진행 상태")]
    [field: SerializeField] public int Score { get; private set; } = 0;
    [field: SerializeField] public int FollowerCount { get; private set; } = 0;
    [field: SerializeField] public int CurrentFloor { get; private set; } = 1;
    [field: SerializeField] public int EndFloor { get; private set; } = 3;
    [field: SerializeField] public bool IsFloorEndReached { get; private set; } = false;

    [Header("배터리 경고 임계값")]
    public float batteryWarningThreshold = 30f;
    public float batteryCriticalThreshold = 20f;

    public float PlayTime { get; private set; } = 0f;

    public event Action<int> OnHealthChanged;
    public event Action<float> OnBatteryChanged;
    public event Action<int> OnScoreChanged;
    public event Action<int> OnFollowerChanged;
    public event Action<bool> OnChangedReachedFloorEnd;

    private void Update()
    {
        PlayTime += Time.deltaTime;

        // 배터리 감소
        BatteryPercent -= batteryDrainRate * Time.deltaTime;
        BatteryPercent = Mathf.Clamp(BatteryPercent, 0f, 100f);
        OnBatteryChanged?.Invoke(BatteryPercent);
    }

    // 체력 감소/회복
    public void ChangeHealth(int amount)
    {
        PlayerHealth = Mathf.Clamp(PlayerHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(PlayerHealth);
    }

    // 점수 증가
    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    // 팔로워 포획
    public void AddFollower(int amount = 1)
    {
        FollowerCount += amount;
        OnFollowerChanged?.Invoke(FollowerCount);
    }

    // 층 이동
    public void MoveToUpFloor()
    {
        CurrentFloor++;
    }

    public void MoveToDownFloor()
    {
        CurrentFloor--;
    }

    public void ReachedFloorEnd()
    {
        IsFloorEndReached = true;
        OnChangedReachedFloorEnd.Invoke(IsFloorEndReached);
    }

    public void ReachedFloorNotEnd()
    {
        IsFloorEndReached = false;
        OnChangedReachedFloorEnd.Invoke(IsFloorEndReached);
    }
}
