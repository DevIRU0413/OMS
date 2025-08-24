using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    public class MSG_NPCRestrictor : MonoBehaviour
    {
        [SerializeField] private LayerMask _npcLayer;
        private Direction _direction;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _npcLayer) != 0)
            {
                if (MSG_NPCProvider.TryGetCatchable(collision, out var catchable))
                {
                    catchable.NPCMoveController.ReachEnd(_direction);
                }
                else if (MSG_NPCProvider.TryGetRival(collision, out var rival))
                {
                    rival.NPCMoveController.ReachEnd(_direction);
                }
                else if (MSG_NPCProvider.TryGetDisturb(collision, out var disturb))
                {
                    disturb.NPCMoveController.ReachEnd(_direction);
                }
            }
        }

        public void SetDirection(Direction direction)
        {
            _direction = direction;
        }
    }
}
