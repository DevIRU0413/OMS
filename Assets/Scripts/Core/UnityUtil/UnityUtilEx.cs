using System.Collections.Generic;
using UnityEngine;

namespace Core.UnityUtil
{
    public static class UnityUtilEx
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            var comp = go.GetComponent<T>();
            if (comp == null)
                comp = go.AddComponent<T>();
            return comp;
        }

        public static GameObject FindOrCreateGameObject(string name)
        {
            GameObject go = GameObject.Find(name);
            if (go == null)
                go = new GameObject(name);
            return go;
        }

        public static T FindNearestTarget<T>(Vector3 origin, List<GameObject> targets) where T : Component
        {
            float minDist = float.MaxValue;
            T nearest = null;

            foreach (var obj in targets)
            {
                if (obj == null) continue;

                var target = obj.GetComponent<T>();
                if (target == null) continue;

                float dist = Vector2.Distance(origin, target.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = target;
                }
            }

            return nearest;
        }
        public static GameObject SafeGetGameObject(IManager m)
        {
            if (IsUnityNull(m)) return null; // 이미 null 또는 Destroy 상태면 null 반환
            try
            {
                return m.GetGameObject();     // 안전하게 시도
            }
            catch (MissingReferenceException)
            {
                return null;                  // 예외 나면 null로 처리
            }
        }

        private static bool IsUnityNull(object obj)
        {
            // C# null 체크
            if (obj == null) return true;

            // UnityEngine.Object 파생 타입이면 Destroy 여부까지 확인
            return (obj is UnityEngine.Object uo && uo == null);
        }

    }
}
