using System;

using UnityEngine;

public enum GameStateType
{
    Init,
    Playing,
    Over,
    Result,
}

public enum ReachedFloorDirection
{
    None,
    Right,
    Left,
}

public class YSJ_GameManager : YSJ_SimpleSingleton<YSJ_GameManager>, IManager
{
    #region IManager
    public bool IsDontDestroy => isDontDestroyOnLoad;

    public void Cleanup() { }
    public GameObject GetGameObject() => this.gameObject;
    public void Initialize()
    {
        StateType = GameStateType.Init;
    }

    #endregion

    public GameStateType StateType { get; private set; } = GameStateType.Init;

    [field: Header("End Branch Score Cat SO")]
    public EndingBranchScoreCatData endCatSO;

    [field: Header("플레이어 상태")]
    [field: SerializeField] private int maxHealth = 100;
    public int CurrentPlayerHealth { get; private set; } = 100;
    [field: SerializeField] public float BatteryPercent { get; private set; } = 100f;
    [field: SerializeField] public float batteryDrainRate { get; private set; } = 1f; // 초당 1% 감소

    [field: Header("게임 진행 상태")]
    [field: SerializeField] public int Score { get; private set; } = 0;
    [field: SerializeField] public int FollowerCount { get; private set; } = 0;
    [field: SerializeField] public int CurrentFloor { get; private set; } = 1;
    [field: SerializeField] public int EndFloor { get; private set; } = 3;
    [field: SerializeField] public bool IsFloorEndReached { get; private set; } = false;
    [field: SerializeField] public ReachedFloorDirection ReachedFloorDirectionType { get; private set; } = ReachedFloorDirection.None;

    [Header("배터리 경고 임계값")]
    public float batteryWarningThreshold = 30f;
    public float batteryCriticalThreshold = 20f;

    public float PlayTime { get; private set; } = 0f;

    public event Action<int> OnHealthChanged;
    public event Action<float> OnBatteryChanged;
    public event Action<int> OnScoreChanged;
    public event Action<int> OnFollowerChanged;
    public event Action<ReachedFloorDirection> OnChangedReachedFloorEnd;

    public event Action OnChangedPlaying;
    public event Action OnChangedOver;
    public event Action OnChangedResult;

    private bool isTimeStopped = false;


    private void Update()
    {
        if (StateType != GameStateType.Playing) return;
        if (isTimeStopped) return;

        PlayTime += Time.deltaTime;

        // 배터리 감소
        BatteryPercent -= batteryDrainRate * Time.deltaTime;
        BatteryPercent = Mathf.Clamp(BatteryPercent, 0f, 100f);
        OnBatteryChanged?.Invoke(BatteryPercent);

        if (BatteryPercent == 0)
            GameOver();
    }

    // 체력 감소/회복
    public void ChangeHealth(int amount)
    {
        CurrentPlayerHealth = Mathf.Clamp(amount, 0, maxHealth);
        OnHealthChanged?.Invoke(CurrentPlayerHealth);
    }

    // 점수 증가
    public void AddScore(int amount)
    {
        Score += Mathf.Max(amount, 0);
        OnScoreChanged?.Invoke(Score);
    }

    // 팔로워 포획
    public void AddFollower(int amount = 1)
    {
        FollowerCount += Mathf.Max(amount, 0);
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

    public void ReachedFloorEnd(ReachedFloorDirection direction)
    {
        IsFloorEndReached = true;
        OnChangedReachedFloorEnd?.Invoke(direction);
    }

    public void ReachedFloorNotEnd()
    {
        IsFloorEndReached = false;
        OnChangedReachedFloorEnd?.Invoke(ReachedFloorDirection.None);
    }

    public void GamePlay()
    {
        StateType = GameStateType.Playing;
        OnChangedPlaying?.Invoke();
    }

    public void GameOver()
    {
        StateType = GameStateType.Over;
        OnChangedOver?.Invoke();
        YSJ_AudioManager.Instance.PlaySfx(MSG.MSG_AudioDict.Get(MSG_AudioClipKey.TimeEnd));
    }

    public void GameResult()
    {
        StateType = GameStateType.Result;
        OnChangedResult?.Invoke();

        YSJ_SystemManager.Instance.LoadSceneWithPreActions(SceneID.SecretEndingScene.ToString());
    }

    // 피버 타임 중 시간 정지 시작
    public void StopBattery()
    {
        isTimeStopped = true;
    }

    // 피버 타임 중 시간 정지 해제
    public void StartBattery()
    {
        isTimeStopped = false;
    }


    private SceneID GetResultEndingScene()
    {

        if (endCatSO == null)
        {
            Debug.Log("EndCatSO is NULL > Direct Chage TitleScene");
            return SceneID.TitleScene;
        }

        var endCat = endCatSO.endingBranchScores;
        for (int i = 0; i < endCat.Count; i++)
        {
            // if (endCat[i].ScoreCat)
        }

        return SceneID.TitleScene;
    }
}
