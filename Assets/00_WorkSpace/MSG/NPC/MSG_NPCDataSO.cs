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
        public          float                   CurrentCharCatchGauge;

        public          float                   CharCatchGaugeHealValue;

        public          float                   CharWalkSpeed;
        public          float                   CharRunSpeed;

        public          int                     CurrentScore;
        public          int                     CatchScore;
        public          int                     ClickScore;
        public          int                     FollowScore;


        public void Init()
        {
            CurrentCharHealth = CharMaxHealth;
            CurrentCharCatchGauge = 0;
            CurrentScore = 0;
        }
    }
}
