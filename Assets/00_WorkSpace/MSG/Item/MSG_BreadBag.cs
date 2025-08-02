using Core.UnityUtil.PoolTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_BreadBag : MonoBehaviour, MSG_ICollectible
    {
        public void Collect()
        {
            MSG_PlayerLogic logic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();

            if (logic.IsWornBreadBag)
            {
                Debug.Log("이미 빵 봉투를 쓰고 있어 return");
                return;
            }

            logic.TryWearBreadBag();
            Vanish();
        }

        private void Vanish()
        {
            // 사라지는 애니메이션 필요 시 넣기
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}
