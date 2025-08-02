using Core.UnityUtil.PoolTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_Heart : MonoBehaviour, MSG_ICollectible
    {
        [SerializeField] private MSG_HeartData _data;

        public void Collect()
        {
            MSG_PlayerLogic logic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
            logic.Heal(_data.HealAmout);
            Vanish();
        }

        private void Vanish()
        {
            // 사라지는 애니메이션 필요 시 넣기
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}
