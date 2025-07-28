using UnityEditor;
using UnityEngine;
using System.IO;

namespace Editor
{
    public class FolderStructureCreator
    {
        [MenuItem("Tools/Create Project Folder Structure")]
        public static void CreateFolders()
        {
            string root = "Assets/Scripts";

            // Core         - 게임 장르에 무관한 핵심 로직
            // Game         - 실제 게임플레이 구현
            // Systems      - UI, 네트워크 등 외부 모듈
            // Editor       - 커스텀 에디터 / 툴
            // Definitions  - ScriptableObject 정의 모음

            string[] folders = new string[]
            {
                $"{root}/Core/Stat",                // 스탯 시스템
                $"{root}/Core/Buff",                // 버프 시스템
                $"{root}/Core/FSM",                 // 상태 머신
                $"{root}/Core/Equipment",           // 장비 시스템
                $"{root}/Core/Shared",              // 유틸성 클래스, 공통 타입

                $"{root}/Game/Unit",                // 플레이어/몬스터 공통 베이스
                $"{root}/Game/Spawner",             // 몬스터 생성기, 웨이브 제어
                $"{root}/Game/Combat",              // 전투 처리 로직
                $"{root}/Game/Skill",               // 스킬/투사체
                $"{root}/Game/Runtime",             // 플레이 중 동작 처리(매니저 등)

                $"{root}/Systems/UI",               // UI
                $"{root}/Systems/Network",          // 네트워크
                $"{root}/Systems/SaveLoad",         // 저장

                $"{root}/Editor/Utilities",         // 유틸 (해당 스크립트 위치)

                // 만들어줄 SO 정의
                $"{root}/Definitions/Buffs",        
                $"{root}/Definitions/Equipments",
                $"{root}/Definitions/Skills",
                $"{root}/Definitions/Units"
            };

            foreach (var folder in folders)
            {
                // 폴더 존재 여부 판단
                if (!Directory.Exists(folder))
                {
                    // 폴더 생성
                    Directory.CreateDirectory(folder);
                    Debug.Log($"생성: {folder}");
                }
            }

            AssetDatabase.Refresh();
            Debug.Log("프로젝트 폴더 구조 생성됨.");
        }
    }
}