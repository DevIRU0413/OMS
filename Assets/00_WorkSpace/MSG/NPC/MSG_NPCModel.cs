using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_NPCArgs
    {
        public int ID;
        public string Name;
        public MSG_CharType CharType;
        public MSG_CatchableNPCtype CatchableNPCtype;
        public float CharAttackDamage;
        public float CharHealth;
        public float CharCatchGauge;
        public float CharCatchGaugeHealValue;
        public float CharWalkSpeed;
        public float CharRunSpeed;
        public int CatchScore;
        public int ClickScore;
        public int FollowScore;
    }

    public class MSG_NPCModel
    {
        public          int                     ID;
        public          string                  Name;

        public          MSG_CharType            CharType;
        public          MSG_CatchableNPCtype    CatchableNPCtype;

        public          float                   CharAttackDamage;

        public          float                   CharMaxHealth;
        public          float                   CurrentCharHealth;

        public          float                   CharMaxCatchGauge;          // 포획 성공하기 위한 최대 게이지
        public          float                   CurrentCharCatchGauge;

        public          float                   CharCatchGaugeHealValue;    // 경쟁 중일 때 초당 게이지를 올릴 수치

        public          float                   CharWalkSpeed;
        public          float                   CharRunSpeed;

        public          int                     CurrentScore;
        public          int                     CatchScore;
        public          int                     ClickScore;
        public          int                     FollowScore;

        public MSG_NPCModel(MSG_NPCArgs args)
        {
            ID = args.ID;
            Name = args.Name;

            CharType = args.CharType;
            CatchableNPCtype = args.CatchableNPCtype;

            CharAttackDamage = args.CharAttackDamage;

            CharMaxHealth = args.CharHealth;
            CurrentCharHealth = CharMaxHealth;

            CharMaxCatchGauge = args.CharCatchGauge;
            CurrentCharCatchGauge = 0;

            CharCatchGaugeHealValue = args.CharCatchGaugeHealValue;

            CharWalkSpeed = args.CharWalkSpeed;
            CharRunSpeed = args.CharRunSpeed;

            CatchScore = args.CatchScore;
            ClickScore = args.ClickScore;
            FollowScore = args.FollowScore;
            ClickScore = args.ClickScore;
            CurrentScore = 0;
        }
    }
}
