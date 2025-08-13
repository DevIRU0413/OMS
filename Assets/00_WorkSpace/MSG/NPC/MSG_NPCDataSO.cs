using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public enum MSG_CharType
    {
        Capturable,
        Rival,
        Boss,
        Disturb
    }

    public enum MSG_CatchableNPCtype
    {
        None,
        Handsome,
        Normal,
        Ugly,
        Special
    }

    // TODO: Field: SerializeField 로 get만 허용해도 좋을 듯
    /// <summary>
    /// CSV 파싱 이전 개발용 NPC 데이터 모델입니다.
    /// </summary>
    [CreateAssetMenu(fileName = "MSG_NPCDataSO", menuName = "ScriptableObjects/MSG_NPCDataSO")]
    public class MSG_NPCDataSO : ScriptableObject
    {
        public          int                     ID;
        public          string                  Name;

        public          MSG_CharType            CharType;
        public          MSG_CatchableNPCtype    CatchableNPCtype;

        public          float                   CharAttackDamage;

        public          float                   CharMaxHealth;
        public          float                   CurrentCharHealth;

        public          float                   CharMaxCatchGauge;
        [SerializeField]
        private         float                   _currentCharCatchGauge;
        public          float                   CurrentCharCatchGauge
        {
            get => _currentCharCatchGauge;
            set
            {
                _currentCharCatchGauge = value;
                OnGaugeChanged?.Invoke(value);
            }
        }

        public          float                   CharCatchGaugeHealValue;

        public          float                   CharWalkSpeed;
        public          float                   CharRunSpeed;

        public          int                     CurrentScore;
        public          int                     CatchScore;     // 포획 직후 얻는 점수, 배율 X
        public          int                     ClickScore;     // 클릭 당 얻는 점수, 배율 O, 라이벌 수와 정비례하니까 애초에 필요 없는 변수인 듯
        public          int                     FollowScore;    // 게임 끝나고 얻는 점수, 배율 X


        public event Action<float> OnGaugeChanged;


        public void Init()
        {
            CurrentCharHealth = CharMaxHealth;
            CurrentCharCatchGauge = 0;
            CurrentScore = 0;
        }
    }
}
